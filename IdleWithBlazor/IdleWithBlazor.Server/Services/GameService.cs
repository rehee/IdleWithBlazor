using IdleWithBlazor.Common.Helpers;
using IdleWithBlazor.Common.Interfaces.Actors;
using IdleWithBlazor.Model.Actions;
using IdleWithBlazor.Model.Actors;
using IdleWithBlazor.Model.GameItems.Items.Equipments;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace IdleWithBlazor.Server.Services
{
  public class GameService : IGameService
  {


    public static ConcurrentDictionary<Guid, IGameRoom> GameRooms { get; set; } = new ConcurrentDictionary<Guid, IGameRoom>();
    public static ConcurrentDictionary<Guid, ICharacter> Characters { get; set; } = new ConcurrentDictionary<Guid, ICharacter>();
    public IEnumerable<IGameRoom> Games()
    {
      foreach (var room in GameRooms.Values)
      {
        yield return room;
      }
    }
    public IEnumerable<ICharacter> GetCharacters()
    {
      foreach (var character in Characters.Values)
      {
        yield return character;
      }
    }
    public IEnumerable<Guid> GetUserWithCharacters()
    {
      foreach (var userId in Characters.Keys)
      {
        yield return userId;
      }
    }
    public Task<ICharacter?> GetCharacterAsync(Guid userId)
    {
      if (Characters.TryGetValue(userId, out var user))
      {
        return Task.FromResult(user);
      }
      return Task.FromResult(default(ICharacter));
    }
    public Task<ICharacter?> CreateCharacterAsync(Guid userId)
    {
      var character = ActorHelper.New<ICharacter>(userId, $"查内姆 {userId.ToString().Split("-")[0]}");
      character.Init();
      if (Characters.TryAdd(userId, character))
      {
        return Task.FromResult(character);
      }
      return null;
    }
    public async Task<IGameRoom?> GetUserRoomAsync(Guid userId)
    {
      var user = await GetCharacterAsync(userId);
      return user?.Room;
    }

    public async Task NewRoomAsync(Guid userId)
    {
      var character = await GetCharacterAsync(userId);
      if (character == null || character.Room != null)
      {
        return;
      }
      var game = await character.CreateRoomAsync();
      await game.CreateMapAsync();
      await game.Map.GenerateMobsAsync();
      GameRooms.AddOrUpdate(userId, game, (b, c) => game);

    }

    public async Task OnTick(IServiceProvider sp)
    {
      await Task.WhenAll(Games().Select(b => b.OnTick(sp)));
    }
  }
}
