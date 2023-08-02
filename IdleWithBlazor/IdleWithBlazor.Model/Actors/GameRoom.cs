using IdleWithBlazor.Common.Helpers;
using IdleWithBlazor.Common.Interfaces.Actors;
using System.Collections.Concurrent;
using System.Text.Json.Serialization;

namespace IdleWithBlazor.Model.Actors
{
  public class GameRoom : Actor, IGameRoom
  {
    public override Type TypeDiscriminator => typeof(GameRoom);
    public override IEnumerable<IActor> Children
    {
      get
      {
        return Map != null ? new IActor[] { Map } : Enumerable.Empty<IActor>();
      }
      set => base.Children = value;
    }
    public Guid? OwnerId { get; set; }
    [JsonIgnore]
    public ICharacters? GameOwner { get; private set; }

    private ConcurrentDictionary<Guid, ICharacters>? guests { get; set; }
    [JsonIgnore]
    public ICharacters[] Guests => guests?.Select(b => b.Value).ToArray() ?? Enumerable.Empty<ICharacters>().ToArray();

    public IGameMap? Map { get; set; }

    public async Task<bool> CloseGameAsync()
    {
      try
      {
        if (Map != null)
        {
          await Map.CloseMapAsync();
        }
        if (guests != null)
        {
          await Task.WhenAll(guests.Keys.Select(b => KickGuestAsync(b)));
        }
        GameOwner = null;
        await DisposeAsync();
        return true;
      }
      catch
      {
        return false;
      }

    }

    public async Task<bool> CreateMapAsync()
    {
      if (Map != null)
      {
        await Map.CloseMapAsync();
      }
      Map = ActorHelper.New<IGameMap>();
      if (Map != null)
      {
        await Map.InitAsync(GameOwner, guests);
        return true;
      }
      return false;
    }

    public Task<bool> InitAsync(ICharacters owner)
    {
      lock (this)
      {
        GameOwner = owner;
        OwnerId = owner.Id;
        guests = new ConcurrentDictionary<Guid, ICharacters>();
        return Task.FromResult(true);
      }
    }

    public Task<bool> JoinGameAsync(ICharacters guest)
    {
      return Task.FromResult(guests?.TryAdd(guest.Id, guest) ?? false);
    }

    public async Task<bool> KickGuestAsync(Guid guestId)
    {
      if (guests?.TryRemove(guestId, out var guest) ?? false)
      {
        return await guest.LeaveGameAsync();
      }
      return false;
    }
  }
}
