﻿using IdleWithBlazor.Common.Interfaces.GameActions;
using IdleWithBlazor.Common.Interfaces.Items;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdleWithBlazor.Common.Interfaces.Actors
{
  public interface ICharacter : IActor, ILevelupable
  {
    Task<IGameRoom?> CreateRoomAsync();
    Task<bool> JoinGameAsync(IGameRoom gameId);
    Task<bool> KickPlayerAsync(Guid playerId);
    Task<bool> LeaveGameAsync();
    Task<bool> UpdatePlayerAsync();
    Task<bool> GainCurrency(int exp);

    ConcurrentDictionary<int, IActionSlot>? ActionSlots { get; }
    


    IGameRoom? Room { get; }
    IPlayer ThisPlayer { get; }
    void Init();
    int BaseAttack { get; }
  }
}
