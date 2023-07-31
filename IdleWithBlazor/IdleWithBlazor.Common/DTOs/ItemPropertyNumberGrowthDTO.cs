using IdleWithBlazor.Common.Enums.Numbers;
using IdleWithBlazor.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdleWithBlazor.Common.DTOs
{
  public class ItemPropertyNumberGrowthDTO
  {
    public ItemPropertyNumberGrowthDTO()
    {

    }
    public ItemPropertyNumberGrowthDTO(string propertyName, double? initialValue, EnumNumberGrowth type, double? grouth)
    {
      PropertyName = propertyName;
      InitialValue = initialValue;
      Type = type;
      Grouth = grouth;
    }

    public string? PropertyName { get; }
    public double? InitialValue { get; }
    public EnumNumberGrowth Type { get; }
    public double? Grouth { get; }

    public NumberGrowth GetNumberGrowth()
    {
      return new NumberGrowth
      {
        BaseValue = InitialValue,
        Growth = Type,
        GrowthNumber = Grouth
      };
    }
  }
}
