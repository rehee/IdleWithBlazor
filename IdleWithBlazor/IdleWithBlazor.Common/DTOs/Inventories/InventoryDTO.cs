using IdleWithBlazor.Common.DTOs.Actors;
using IdleWithBlazor.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdleWithBlazor.Common.DTOs.Inventories
{
  public class InventoryDTO : ActorDTO
  {
    public GameItemDTO[]? Items { get; set; }
    public EquiptorDTO? Equiptor { get; set; }
  }
}
