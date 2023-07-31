using IdleWithBlazor.Common.Enums.Numbers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdleWithBlazor.Common.Models
{
  public class NumberGrowth
  {
    public EnumNumberGrowth Growth { get; set; }

    public double? BaseValue { get; set; }
    public double? GrowthNumber { get; set; }
    public double? GetValue(int itemLevel)
    {
      if (!GrowthNumber.HasValue)
      {
        return null;
      }
      switch (Growth)
      {
        case EnumNumberGrowth.Linear:
          return BaseValue + (itemLevel - 1) * GrowthNumber;
        case EnumNumberGrowth.Logarithmic:
          return BaseValue.Value * Math.Pow(1 + GrowthNumber.Value, itemLevel - 1);
      }
      return null;
    }
  }
}
