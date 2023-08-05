using IdleWithBlazor.Common.Enums;
using IdleWithBlazor.Common.Interfaces.Actors;
using IdleWithBlazor.Model.Actors;

namespace IdleWithBlazor.Server.Services
{
  public interface IGameService
  {
    Task NewRoomAsync(Guid userId);
    Task<IGameRoom?> GetUserRoomAsync(Guid userId);
    Task<ICharacter?> GetCharacterAsync(Guid userId);
    Task<ICharacter?> CreateCharacterAsync(Guid userId);
    IEnumerable<ICharacter> GetCharacters();
    IEnumerable<Guid> GetUserWithCharacters();
    IEnumerable<IGameRoom> Games();
    Task JoinGame(Guid userId, Guid id);
    Task QuitGame(Guid userId);

    Task<bool> EquipOrUnequip(Guid userId, Guid? itemId, int? offset, EnumEquipmentSlot? slot);
    Task SelectSkill(Guid userId, Guid skillId, int slot);

    Task OnTick(IServiceProvider sp);

  }
}
