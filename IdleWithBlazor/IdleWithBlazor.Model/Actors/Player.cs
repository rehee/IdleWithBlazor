using IdleWithBlazor.Common.Interfaces.Actors;
using IdleWithBlazor.Common.Interfaces.GameActions;
using IdleWithBlazor.Common.Interfaces.Items;
using IdleWithBlazor.Model.Characters;
using System.Collections.Concurrent;

namespace IdleWithBlazor.Model.Actors
{
  public class Player : Sprite, IPlayer
  {
    public override Type TypeDiscriminator => typeof(Player);

    IActionSkill[]? ISprite.ActionSkills => throw new NotImplementedException();

    public void SetActions(IActionSkill[]? skills)
    {
      throw new NotImplementedException();
    }

    public void SetPlayerFromCharacter(ICharacters character)
    {
      this.Id = character.Id;
      this.Name = character.Name;
      SetActionSlots(character.ActionSlots);
    }
  }
}
