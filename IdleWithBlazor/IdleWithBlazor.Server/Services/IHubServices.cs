using IdleWithBlazor.Common.Enums;
using IdleWithBlazor.Model.Actors;

namespace IdleWithBlazor.Server.Services
{
  public interface IHubServices : IDisposable, IAsyncDisposable
  {
    Task UserConnected(Guid userId, string connectionId);
    Task UserLeave(string connectionId);
    Task SetUserPage(string connectionId, EnumUserPage page);
    Task Broadcast(IEnumerable<GameRoom> games);
    Task<IEnumerable<Guid>> ConnectedUsers();
  }
}
