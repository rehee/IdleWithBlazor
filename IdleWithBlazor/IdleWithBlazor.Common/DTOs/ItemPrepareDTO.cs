using IdleWithBlazor.Common.Enums;
using IdleWithBlazor.Common.Interfaces.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdleWithBlazor.Common.DTOs
{
  public class ItemPrepareDTO
  {
    public ITemplate Template { get; set; }
    public EnumItemQuality Quality { get; set; }
    public int ItemLevel { get; set; }
  }
}
