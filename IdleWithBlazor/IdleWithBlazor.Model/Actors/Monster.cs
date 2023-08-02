using IdleWithBlazor.Common.Interfaces.Actors;
using IdleWithBlazor.Common.Interfaces.GameActions;

namespace IdleWithBlazor.Model.Actors
{
  public class Monster : Sprite, IMonster
  {
    public override Type TypeDiscriminator => typeof(Monster);

    IActionSkill[]? ISprite.ActionSkills => throw new NotImplementedException();

    public void SetActions(IActionSkill[]? skills)
    {
      throw new NotImplementedException();
    }
  }
}
