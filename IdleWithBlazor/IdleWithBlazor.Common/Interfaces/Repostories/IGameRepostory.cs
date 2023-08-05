using IdleWithBlazor.Common.Interfaces.Actors;

namespace IdleWithBlazor.Common.Interfaces.Repostories
{
  public interface IGameRepostory
  {
    IEnumerable<IGameRoom> Roomes { get; }
    Task<bool> AddRoomAsync(IGameRoom room);
    Task<bool> RemoveRoomAsync(Guid roomId);

    IEnumerable<ICharacter> Characters { get; }
    IEnumerable<Guid> CharacterIds { get; }
    Task<ICharacter?> FindCharacterAsync(Guid characterId);
    Task<bool> AddCharacterAsync(ICharacter character);
    Task<bool> RemoveCharacterAsync(Guid characterId);
  }
}
