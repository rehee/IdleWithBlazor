using IdleWithBlazor.Common.Enums;
using IdleWithBlazor.Common.Interfaces.Items;
using System.Collections.Concurrent;

namespace IdleWithBlazor.Common.Interfaces.Actors
{
  public interface IEquiptor : IActor
  {
    IEnumerable<(EnumEquipmentSlot slot, IEquipment equipment)> Equipments();
    bool Equip(IEquipment equip, int? offset = null);
    IEnumerable<IEquipment?> UnEquip(params EnumEquipmentSlot[] equips);
  }
}
