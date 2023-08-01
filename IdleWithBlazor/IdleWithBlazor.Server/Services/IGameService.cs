using IdleWithBlazor.Model.Actors;

namespace IdleWithBlazor.Server.Services
{
  public interface IGameService
  {
    Task NewRoomAsync(Guid userId);
    Task<GameRoom> GetUserRoomAsync(Guid userId);
    IEnumerable<GameRoom> Games();
    Task OnTick(IServiceProvider sp);
  }
}
