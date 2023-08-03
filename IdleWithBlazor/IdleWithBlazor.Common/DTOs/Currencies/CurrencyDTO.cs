using IdleWithBlazor.Common.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdleWithBlazor.Common.DTOs.Currencies
{
  public class CurrencyDTO
  {
    public EnumGameCurrency CurrencyType { get; set; }
    public int Amount { get; set; }
  }
}
