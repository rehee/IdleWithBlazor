using IdleWithBlazor.Common.Enums;

namespace IdleWithBlazor.Common.Interfaces.Repostories
{
  public interface IHubConnectionRepostory
  {
    Task<bool> AddConnectionIdAsync(string connectionId, Guid userId, EnumUserPage page = EnumUserPage.Character);
    Task<bool> RemoveConnectionIdAsync(string connectionId);
    Task<bool> UpdateConnectionPagesAsync(string connectionId, EnumUserPage page);
    Task<Guid?> FindConnectedUser(string connectionId);
    IEnumerable<Guid> Connectedusers { get; }
    IEnumerable<(Guid userId, string connectionId, EnumUserPage page)> ConnectionUsers { get; }
  }
}
