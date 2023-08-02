using IdleWithBlazor.Common.Enums;
using IdleWithBlazor.Common.Interfaces.Items;
using System.Collections.Concurrent;

namespace IdleWithBlazor.Common.Interfaces.Actors
{
  public interface IEquiptor : IActor
  {
    ConcurrentDictionary<EnumEquipmentSlot, IEquipment> Equipments { get; set; }
  }
}
