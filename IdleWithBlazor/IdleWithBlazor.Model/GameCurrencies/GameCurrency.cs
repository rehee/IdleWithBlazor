using IdleWithBlazor.Common.Enums;
using IdleWithBlazor.Common.Interfaces.Currencies;
using IdleWithBlazor.Model.Actors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdleWithBlazor.Model.GameCurrencies
{
  public class GameCurrency : Actor, IGameCurrency
  {
    public override Type TypeDiscriminator => typeof(GameCurrency);
    public EnumGameCurrency Currency { get; set; }

    public int Amount { get; set; }
    public Task<bool> ConsumeTokens(int amount)
    {
      if (Amount < amount)
      {
        return Task.FromResult(false);
      }
      Amount = Amount - amount;
      return Task.FromResult(true);
    }

    public Task<bool> ReceiveTokens(int amount)
    {
      Amount = Amount + amount;
      return Task.FromResult(true);
    }
  }
}
