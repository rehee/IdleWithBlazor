using IdleWithBlazor.Common.Enums;
using IdleWithBlazor.Common.Interfaces.Actors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdleWithBlazor.Common.Interfaces.Items
{
  public interface IEquipment : IGameItem, IEquipProperty
  {
    EnumEquipment EquipmentType { get; set; }
    EnumItemQuality ItemQuality { get; set; }

    
  }
}
