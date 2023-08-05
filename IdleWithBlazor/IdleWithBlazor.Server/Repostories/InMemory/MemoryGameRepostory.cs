using IdleWithBlazor.Common.Interfaces.Actors;
using IdleWithBlazor.Common.Interfaces.Repostories;
using IdleWithBlazor.Model.Characters;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace IdleWithBlazor.Server.Repostories.InMemory
{
  public class MemoryGameRepostory : IGameRepostory
  {
    private static ConcurrentDictionary<Guid, IGameRoom> gameRooms { get; set; }
    private static ConcurrentDictionary<Guid, ICharacter> characters { get; set; }

    public MemoryGameRepostory()
    {
      gameRooms = new ConcurrentDictionary<Guid, IGameRoom>();
      characters = new ConcurrentDictionary<Guid, ICharacter>();
    }
    public IEnumerable<IGameRoom> Roomes => gameRooms.Values;

    public IEnumerable<ICharacter> Characters => characters.Values;
    public IEnumerable<Guid> CharacterIds => characters.Keys;
    public Task<ICharacter?> FindCharacterAsync(Guid characterId)
    {
      characters.TryGetValue(characterId, out var result);
      return Task.FromResult(result);
    }
    public Task<bool> AddCharacterAsync(ICharacter character)
    {
      return Task.FromResult(characters.TryAdd(character.Id, character));
    }

    public Task<bool> AddRoomAsync(IGameRoom room)
    {
      return Task.FromResult(gameRooms.TryAdd(room.Id, room));
    }

    public Task<bool> RemoveCharacterAsync(Guid characterId)
    {
      return Task.FromResult(characters.TryRemove(characterId, out var character));
    }

    public Task<bool> RemoveRoomAsync(Guid roomId)
    {
      return Task.FromResult(gameRooms.TryRemove(roomId, out var room));
    }
  }
}
