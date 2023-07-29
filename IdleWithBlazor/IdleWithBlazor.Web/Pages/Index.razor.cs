using IdleWithBlazor.Web.Components;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;

namespace IdleWithBlazor.Web.Pages
{
  public class IndexPage : PageBase
  {
    public string m { get; set; }
    public async Task Send()
    {
      await connection.ConnectionAsync();
      await connection.Send();
    }
    public async Task Keep()
    {
      await connection.ConnectionAsync();
      await connection.KeepSend();
    }
    public async Task GetRoom()
    {
      await connection.ConnectionAsync();
      await connection.GetRoom();
    }

  }
}
