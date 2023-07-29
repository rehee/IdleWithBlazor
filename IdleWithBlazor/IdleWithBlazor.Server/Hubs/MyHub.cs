using Microsoft.AspNetCore.SignalR;

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
      await Clients.All.SendAsync("ReceiveMessage", "", this.GetHashCode());
    }
    public async Task SendMessage(string user, string message)
    {
      //for (var i = 0; i < 10; i++)
      //{
      //  await Clients.All.SendAsync("ReceiveMessage", user, Guid.NewGuid().ToString());
      //  await Task.Delay(1000);
      //}

    }
  }
}
