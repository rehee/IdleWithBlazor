using IdleWithBlazor.Common.Interfaces.Actors;
using IdleWithBlazor.Common.Interfaces.Items;
using IdleWithBlazor.Model.Characters;
using System.Collections.Concurrent;

namespace IdleWithBlazor.Model.Actors
{
  public class Player : Sprite, IPlayer
  {
    public override Type TypeDiscriminator => typeof(Player);

    

    public void SetPlayerFromCharacter(ICharacters character)
    {
      this.Id = character.Id;
      this.Name = character.Name;
    }
  }
}
