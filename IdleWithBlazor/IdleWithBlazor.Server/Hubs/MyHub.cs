using IdleWithBlazor.Common.Consts;
using IdleWithBlazor.Common.Helpers;
using IdleWithBlazor.Server.Services;
using IdleWithBlazor.Server.Tasks;
using Microsoft.AspNetCore.SignalR;
using System.Text.Json;

namespace IdleWithBlazor.Server.Hubs
{
  public class MyHub : Hub
  {
    public MyHub()
    {

    }
    public override async Task OnConnectedAsync()
    {
      await base.OnConnectedAsync();
      Console.WriteLine("connected");
      var context = this.Context;
      var accessToken = Context.GetHttpContext()?.Request.Query["access_token"];
      var i = context.UserIdentifier;
      var id = context.ConnectionId;
      Console.WriteLine(this.GetHashCode());
      await Clients.Caller.SendAsync("ReceiveMessage", "", $"{this.GetHashCode()},{context.ConnectionId}");
      //context.Abort();

    }
    public async Task SendMessage(string user, string message)
    {
      await Clients.Caller.SendAsync("ReceiveMessage", $"{this.GetHashCode()}", $"{this.GetHashCode()},{Context.ConnectionId}");
    }
    public async Task Stop(string user, string message)
    {
      Context.Abort();
    }

    public async Task GetRoom()
    {
      await Clients.Caller.SendAsync("RoomMessage", JsonSerializer.Serialize(GameService.Room, JsonSerializerOptionsHelper.Default));
    }

    public async Task KeepSend()
    {
      Console.WriteLine($"{this.GetHashCode()},{Context.ConnectionId} keep send click");
      //while (!Context.ConnectionAborted.IsCancellationRequested)
      //{
      //  await Clients.Caller.SendAsync("ReceiveMessage", $"{this.GetHashCode()}", $"{this.GetHashCode()},{Context.ConnectionId}");
      //  await Task.Delay(ConstSetting.TickTime / 2);
      //}
      Task.Run(async () =>
      {
        while (true)
        {
          await Clients.Caller.SendAsync("ReceiveMessage", $"{this.GetHashCode()}", $"{this.GetHashCode()},{Context.ConnectionId}");
          await Task.Delay(ConstSetting.TickTime / 2);
        }
      });
      await Clients.Caller.SendAsync("ReceiveMessage", $"{this.GetHashCode()}", $"{this.GetHashCode()},{Context.ConnectionId}");

      Console.WriteLine("aborted");
    }
  }
}
