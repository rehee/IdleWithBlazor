using IdleWithBlazor.Common.Consts;
using IdleWithBlazor.Common.Enums;
using IdleWithBlazor.Common.Helpers;
using IdleWithBlazor.Server.Services;
using IdleWithBlazor.Server.Tasks;
using Microsoft.AspNetCore.SignalR;
using System.Text.Json;

namespace IdleWithBlazor.Server.Hubs
{
  public class MyHub : Hub
  {
    private readonly IHubServices service;

    public MyHub(IHubServices service)
    {
      this.service = service;
    }
    public override async Task OnConnectedAsync()
    {
      await base.OnConnectedAsync();
      var accessToken = Context.GetHttpContext()?.Request.Query["access_token"];
      if (String.IsNullOrWhiteSpace(accessToken))
      {
        Context.Abort();
      }
      var userId = Guid.Parse(accessToken);
      await service.UserConnected(userId, Context.ConnectionId);
    }
    public async Task SendMessage(string user, string message)
    {
      await Clients.Caller.SendAsync("ReceiveMessage", $"{this.GetHashCode()}", $"{this.GetHashCode()},{Context.ConnectionId}");
    }
    public async Task Stop(string user, string message)
    {
      Context.Abort();
    }

    public async Task SetUserPage(EnumUserPage page)
    {
      await service.SetUserPage(Context.ConnectionId, page);
    }


    public override async Task OnDisconnectedAsync(Exception? exception)
    {
      await service.UserLeave(Context.ConnectionId);
      await base.OnDisconnectedAsync(exception);
    }
    public async Task<bool> EquipItem(Guid? id, int? offset)
    {
      return await service.EquipOrUnequip(Context.ConnectionId, id, offset, null);
    }
    public async Task<bool> UnEquipItem(EnumEquipmentSlot slot)
    {
      return await service.EquipOrUnequip(Context.ConnectionId, null, null, slot);
    }
  }
}
