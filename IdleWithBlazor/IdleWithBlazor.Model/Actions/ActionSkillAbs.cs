using IdleWithBlazor.Common.Helpers;
using IdleWithBlazor.Common.Interfaces.GameActions;
using IdleWithBlazor.Model.Actors;

namespace IdleWithBlazor.Model.Actions
{
    public abstract class ActionSkillAbs : GameAction
  {
    public abstract decimal CoolDown { get; set; }
    public abstract decimal AttackSpeed { get; set; }
    public int CoolDownTickRemain { get; set; }
    public int CoolDownTick { get; set; }

    public (bool isType, T value) ActorType<T>() where T : Sprite
    {
      if (Parent is T p)
      {
        return (true, p);
      }
      return (false, default(T));
    }

    public virtual void SetCooldownTick()
    {
      CoolDownTick = TickHelper.GetColdDownTick(AttackSpeed, CoolDown);
    }

    public override Task<bool> OnTick(IServiceProvider sp)
    {
      if (CoolDownTickRemain > 0)
      {
        CoolDownTickRemain--;
        return Task.FromResult(false);
      }
      CoolDownTickRemain = CoolDownTick;
      return Task.FromResult(true); ;

    }
  }
}
