using IdleWithBlazor.Common.Interfaces.Actors;

namespace IdleWithBlazor.Common.Interfaces.GameActions
{
  public interface IActionSkill : IGameAction, IActionSkillInfo
  {
    Task<bool> Attack(ISprite attacker, IEnumerable<ISprite> targets);
  }
}
