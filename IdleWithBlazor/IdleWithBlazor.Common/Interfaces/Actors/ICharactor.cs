using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdleWithBlazor.Common.Interfaces.Actors
{
  public interface ICharacters : IActor
  {
    Task<IGameRoom?> CreateRoomAsync();
    Task<bool> JoinGameAsync(IGameRoom gameId);
    Task<bool> KickPlayerAsync(Guid playerId);
    Task<bool> LeaveGameAsync();
    Task<bool> UpdatePlayerAsync();

    IGameRoom? Room { get; }
    IPlayer ThisPlayer { get; }

  }
}
