using IdleWithBlazor.Common.Interfaces.Actors;
using IdleWithBlazor.Common.Interfaces.Items;
using IdleWithBlazor.Model.Characters;
using System.Collections.Concurrent;

namespace IdleWithBlazor.Model.Actors
{
  public class Player : Sprite
  {
    public override Type TypeDiscriminator => typeof(Player);

    public Equiptor Inventory { get; set; }

    public void PickItem(IGameItem item)
    {
      if (Inventory == null)
      {
        Inventory = new Equiptor();
      }
      if (item is IEquipment ep)
      {
        Inventory.Equip(ep);
      }



    }
  }
}
