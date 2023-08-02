using IdleWithBlazor.Common.Interfaces.Actors;
using IdleWithBlazor.Model.Actors;

namespace IdleWithBlazor.Server.Services
{
  public interface IGameService
  {
    Task NewRoomAsync(Guid userId);
    Task<IGameRoom> GetUserRoomAsync(Guid userId);
    IEnumerable<IGameRoom> Games();
    Task OnTick(IServiceProvider sp);
  }
}
