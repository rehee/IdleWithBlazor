using IdleWithBlazor.Common.DTOs.Actors;
using IdleWithBlazor.Common.Enums;
using IdleWithBlazor.Common.Interfaces.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdleWithBlazor.Common.DTOs.Inventories
{
  public class EquiptorDTO : ActorDTO
  {
    public Dictionary<EnumEquipmentSlot, EquipmentDTO>? Equpments { get; set; }
  }
}
