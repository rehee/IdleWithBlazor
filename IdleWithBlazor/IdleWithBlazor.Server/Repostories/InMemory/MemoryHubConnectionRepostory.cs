using IdleWithBlazor.Common.Enums;
using IdleWithBlazor.Common.Interfaces.Repostories;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Concurrent;

namespace IdleWithBlazor.Server.Repostories.InMemory
{
  public class MemoryHubConnectionRepostory : IHubConnectionRepostory
  {
    ConcurrentDictionary<string, Guid> connectionIdUserMap { get; set; }
    ConcurrentDictionary<string, EnumUserPage> connectionIdUserPageMap { get; set; }
    public MemoryHubConnectionRepostory()
    {
      connectionIdUserMap = new ConcurrentDictionary<string, Guid>();
      connectionIdUserPageMap = new ConcurrentDictionary<string, EnumUserPage>();

    }
    public IEnumerable<Guid> Connectedusers => connectionIdUserMap.Values;

    public IEnumerable<(Guid userId, string connectionId, EnumUserPage page)> ConnectionUsers =>
      (from connections in connectionIdUserMap.Select(b => (b.Key, b.Value))
       join pages in connectionIdUserPageMap.Select(b => (b.Key, b.Value)) on connections.Key equals pages.Key into ug
       from pages in ug.DefaultIfEmpty()
       select (connections.Value, connections.Key, pages.Value));

    public Task<bool> AddConnectionIdAsync(string connectionId, Guid userId, EnumUserPage page = EnumUserPage.Character)
    {
      connectionIdUserMap.TryAdd(connectionId, userId);
      connectionIdUserPageMap.TryAdd(connectionId, page);
      return Task.FromResult(true);
    }

    public Task<Guid?> FindConnectedUser(string connectionId)
    {
      if (connectionIdUserMap.TryGetValue(connectionId, out var userId))
      {
        return Task.FromResult<Guid?>(userId);
      }
      return Task.FromResult<Guid?>(null);
    }

    public Task<bool> RemoveConnectionIdAsync(string connectionId)
    {
      connectionIdUserMap.TryRemove(connectionId, out var userId);
      connectionIdUserPageMap.TryRemove(connectionId, out var page);
      return Task.FromResult(true);
    }

    public Task<bool> UpdateConnectionPagesAsync(string connectionId, EnumUserPage page)
    {
      connectionIdUserPageMap.AddOrUpdate(connectionId, page, (p, a) => page);
      return Task.FromResult(true);
    }
  }
}
