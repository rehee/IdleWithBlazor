using IdleWithBlazor.Common.Enums;
using IdleWithBlazor.Common.Interfaces.Actors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdleWithBlazor.Common.Interfaces.Currencies
{
  public interface IGameCurrency : IActor
  {
    EnumGameCurrency Currency { get; }
    int Amount { get; }
    Task<bool> ReceiveTokens(int amount);
    Task<bool> ConsumeTokens(int amount);
  }
}
