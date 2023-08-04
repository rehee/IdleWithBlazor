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

    public int CurrentExp { get; set; }
    public int NextLevelExp { get; set; }
    public bool EnableLevelUp { get; set; }


    public void SetPlayerFromCharacter(ICharacter character)
    {
      this.Id = character.Id;
      this.Name = character.Name;
      this.Level = character.Level;
      this.CurrentExp = character.CurrentExp;
      this.NextLevelExp = character.NextLevelExp;
      this.MinAttack = character.BaseAttack;
      this.MaxAttack = character.BaseAttack;
      this.MaxHp = 100 + 100 * (character.Level - 1);
      this.CurrentHp = MaxHp;
      SetActionSlots(character.ActionSlots);
    }

  }
}
