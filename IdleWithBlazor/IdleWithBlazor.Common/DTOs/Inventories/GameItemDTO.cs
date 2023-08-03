using IdleWithBlazor.Common.DTOs.Actors;
using IdleWithBlazor.Common.Enums;
using IdleWithBlazor.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdleWithBlazor.Common.DTOs.Inventories
{
  public class GameItemDTO : ActorDTO
  {
    public EnumItemType Type { get; set; }
    public EnumEquipment EquipType { get; set; }

  }
}
