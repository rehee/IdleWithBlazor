using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdleWithBlazor.Common.Interfaces.Actors
{
  public interface IGameRoom : IActor
  {
    Task<bool> InitAsync(ICharacter owner);
    Task<bool> JoinGameAsync(ICharacter guest);
    Task<bool> KickGuestAsync(Guid guestId);
    Task<bool> CreateMapAsync();
    Task<bool> CloseGameAsync();

    Guid? OwnerId { get; }
    ICharacter? GameOwner { get; }
    ICharacter[]? Guests { get; }
    IGameMap? Map { get; }

  }
}
