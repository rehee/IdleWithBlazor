using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdleWithBlazor.Common.Interfaces.Actors
{
  public interface IGameRoom : IActor
  {
    Task<bool> InitAsync(ICharacters owner);
    Task<bool> JoinGameAsync(ICharacters guest);
    Task<bool> KickGuestAsync(Guid guestId);
    Task<bool> CreateMapAsync();
    Task<bool> CloseGameAsync();

    Guid? OwnerId { get; }
    ICharacters? GameOwner { get; }
    ICharacters[] Guests { get; }
    IGameMap? Map { get; }

  }
}
