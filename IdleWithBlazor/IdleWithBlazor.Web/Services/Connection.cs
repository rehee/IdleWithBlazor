using Microsoft.AspNetCore.SignalR.Client;

namespace IdleWithBlazor.Web.Services
{
  public class Connection : IConnection
  {
    private readonly IStorageService storage;

    private HubConnection? hub { get; set; } = null;
    public Connection(IStorageService storage)
    {
      this.storage = storage;
    }
    public async Task<bool> AbortAsync()
    {
      if (hub != null)
      {
        try
        {
          await hub.DisposeAsync();
        }
        catch
        {

        }
      }
      return true;
    }

    public async Task<bool> ConnectionAsync()
    {
      if (hub != null)
      {
        return true;
      }
      var token = await storage.GetAsync<string?>("token");
      if (string.IsNullOrEmpty(token))
      {
        return false;
      }
      hub = new HubConnectionBuilder()
       .WithUrl("https://localhost:7026/myhub", options =>
       {
         options.AccessTokenProvider = () => Task.FromResult<string?>(token);
       })

       .Build();
      try
      {
        await hub.StartAsync();
        return true;
      }
      catch
      {
        return false;
      }

    }
  }
}
