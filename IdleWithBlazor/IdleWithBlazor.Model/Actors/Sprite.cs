using IdleWithBlazor.Common.Interfaces.Actors;
using IdleWithBlazor.Common.Interfaces.GameActions;
using System.Numerics;

namespace IdleWithBlazor.Model.Actors
{
  public abstract class Sprite : Actor, ISprite
  {
    public virtual BigInteger MaxHp { get; set; }
    public virtual BigInteger CurrentHp { get; set; }
    public int MinAttack { get; set; }
    public int MaxAttack { get; set; }
    public virtual IActor Target { get; protected set; }
    public IActionSlot? ActionSlots { get; set; }

    protected void SetActionSlots(IActionSlot? actionSlots)
    {
      ActionSlots = actionSlots;
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


    IActionSkill[]? ISprite.ActionSkills => throw new NotImplementedException();

    public override async Task<bool> OnTick(IServiceProvider sp)
    {
      if (ActionSkills?.Any() == true)
      {
        if (ActionSlots == null)
        {
          return false;
        }

        return (await Task.WhenAll(ActionSlots.OnTick(sp))).All(b => b == true);
      }
      return await base.OnTick(sp);

    }
    //(
    //(
    ////new Task<bool>[] { base.OnTick() })
    ////.Concat(ActionSkills.Select(b => b.OnTick()))
    ////.ToArray()
    ////ActionSkills.Select(b => b.OnTick(sp)).ToArray()
    //)))
    public void SetActions(IActionSkill[]? skills)
    {
      throw new NotImplementedException();
    }
  }
}
