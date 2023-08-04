using IdleWithBlazor.Common.Interfaces.Actors;
using IdleWithBlazor.Common.Interfaces.GameActions;
using System.Collections.Concurrent;
using System.Numerics;

namespace IdleWithBlazor.Model.Actors
{
  public abstract class Sprite : Actor, ISprite
  {
    public int Level { get; set; }
    public virtual BigInteger MaxHp { get; set; }
    public virtual BigInteger CurrentHp { get; set; }
    public int MinAttack { get; set; }
    public int MaxAttack { get; set; }
    public virtual IActor Target { get; protected set; }
    public IEnumerable<IActionSlot>? ActionSlots => slots?.Values;
    protected ConcurrentDictionary<int, IActionSlot>? slots { get; set; }
    protected void SetActionSlots(ConcurrentDictionary<int, IActionSlot>? actionSlots)
    {
      slots = actionSlots;
    }
    public virtual void SetTarget(IActor target)
    {
      Target = target;
    }
    public virtual void SetAction(IActionSkill skill)
    {
      this.ActionSkills = new List<IActionSkill>() { skill };
      skill.SetParent(this);
    }
    public virtual IEnumerable<IActor> ActionSkills { get; set; }

    public override async Task<bool> OnTick(IServiceProvider sp)
    {
      if (ActionSlots == null)
      {
        return false;
      }

      return (await Task.WhenAll(ActionSlots.Where(b => b != null).Select(b => b?.OnTick(sp)))).All(b => b == true);
    }
  }
}
