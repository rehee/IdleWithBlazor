using IdleWithBlazor.Common.Interfaces.Actors;
using IdleWithBlazor.Model.Actions;
using System.Numerics;

namespace IdleWithBlazor.Model.Actors
{
  public abstract class Sprite : Actor, ISprite
  {
    public virtual BigInteger MaxHp { get; set; }
    public virtual BigInteger CurrentHp { get; set; }
    public virtual IActor Target { get; protected set; }
    public virtual void SetTarget(IActor target)
    {
      Target = target;
    }


    public virtual void SetActions(IActionSkill[]? skills)
    {
      this.ActionSkills = skills;
      if (skills == null)
      {
        return;
      }
      foreach (var skill in skills)
      {
        skill.SetParent(this);
      }
    }
    public virtual IActionSkill[]? ActionSkills { get; protected set; }

    public override async Task<bool> OnTick(IServiceProvider sp)
    {
      if (ActionSkills?.Any() == true)
      {
        return (
          await Task.WhenAll(
          (
          ActionSkills.Select(b => b.OnTick(sp)).ToArray()
          )))
          .All(b => b == true);
      }
      return await base.OnTick(sp);

    }

  }
}
