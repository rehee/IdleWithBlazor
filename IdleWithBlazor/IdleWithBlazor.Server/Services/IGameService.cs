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
    Task OnTick(IServiceProvider sp);
  }
}
