using IdleWithBlazor.Common.Consts;
using IdleWithBlazor.Common.Enums;
using IdleWithBlazor.Common.Helpers;
using IdleWithBlazor.Common.Interfaces.Actors;
using IdleWithBlazor.Model.Actors;
using IdleWithBlazor.Server.Hubs;
using Microsoft.AspNetCore.SignalR;
using System.Collections.Concurrent;
using System.Runtime.CompilerServices;
using System.Text.Json;

namespace IdleWithBlazor.Server.Services
{
  public class HubServices : IHubServices
  {
    private readonly IHubContext<Hub> hubContext;
    static ConcurrentDictionary<string, Guid> ConnectionIdUserMap = new ConcurrentDictionary<string, Guid>();
    static ConcurrentDictionary<string, EnumUserPage> ConnectionIdUserPageMap = new ConcurrentDictionary<string, EnumUserPage>();
    public HubServices(IHubContext<Hub> hubContext)
    {
      this.hubContext = hubContext;
    }
    public Task UserConnected(Guid userId, string connectionId)
    {
      ConnectionIdUserMap.TryAdd(connectionId, userId);
      ConnectionIdUserPageMap.TryAdd(connectionId, EnumUserPage.Character);
      return Task.CompletedTask;
    }
    public Task UserLeave(string connectionId)
    {
      ConnectionIdUserMap.TryRemove(connectionId, out var id);
      ConnectionIdUserPageMap.TryRemove(connectionId, out var page);
      return Task.CompletedTask;
    }
    public Task SetUserPage(string connectionId, EnumUserPage page)
    {
      ConnectionIdUserPageMap.AddOrUpdate(connectionId, page, (k, v) => page);
      return Task.CompletedTask;
    }
    public async Task Broadcast(IEnumerable<IGameRoom> games)
    {
      var users = ConnectionIdUserMap
        .Select(b =>
        (
          connectionId: b.Key,
          userId: b.Value,
          game: games.FirstOrDefault(g => g.OwnerId.Equals(b.Value)))
        ).ToArray();
      var connectionGroup = ConnectionIdUserPageMap.Select(b => (id: b.Key, type: b.Value));
      var query =
        from c in connectionGroup
        join u in users on c.id equals u.connectionId into ug
        from user in ug.DefaultIfEmpty()
        select
        new
        {
          UserId = user.userId,
          ConnectionId = user.connectionId,
          Geme = user.game,
          Page = c.type,
          Client = hubContext.Clients.Client(user.connectionId)
        };

      await Task.WhenAll(query.Select(q =>
      {
        switch (q.Page)
        {
          case EnumUserPage.Combat:
            return q.Client.SendAsync("CombatMessage", JsonSerializer.Serialize(q.Geme, ConstSetting.Options));
          default:
            return Task.CompletedTask;
        }
      }));
      await Task.CompletedTask;
    }


    bool IsDispose { get; set; }

    public Task<IEnumerable<Guid>> ConnectedUsers()
    {
      var result = ConnectionIdUserMap.Values.Select(b => b).ToArray() ?? Enumerable.Empty<Guid>();
      return Task.FromResult(result);
    }

    public void Dispose()
    {
      if (IsDispose) return;
      IsDispose = true;
      GC.SuppressFinalize(this);
    }

    public ValueTask DisposeAsync()
    {
      Dispose();
      return ValueTask.CompletedTask;
    }

  }
}
