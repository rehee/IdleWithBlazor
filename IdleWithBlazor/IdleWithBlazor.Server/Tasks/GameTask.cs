using IdleWithBlazor.Common.Consts;
using IdleWithBlazor.Common.Helpers;
using IdleWithBlazor.Server.Hubs;
using IdleWithBlazor.Server.Services;
using Microsoft.AspNetCore.SignalR;
using System.Text.Json;

namespace IdleWithBlazor.Server.Tasks
{
  public class GameTask : BackgroundService
  {
    private readonly IGameService service;
    private readonly IServiceProvider sp;

    IHubServices? hubService { get; set; }
    public GameTask(IGameService service, IServiceProvider sp)
    {
      this.service = service;
      this.sp = sp;
    }
    int count = 0;
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
      if (hubService == null)
      {
        using var scope = sp.CreateScope();
        hubService = scope.ServiceProvider.GetService<IHubServices>();
      }
      var connectedUsers = await hubService.ConnectedUsers();
      var currentGames = service.Games();
      var userWithNoGame = connectedUsers.Where(b => !currentGames.Select(b => b.OwnerId).Contains(b)).ToArray();
      await Task.WhenAll(userWithNoGame.Select(b => service.NewRoomAsync(b)));

      await service.OnTick();
      await hubService.Broadcast(service.Games());
      await Task.Delay(ConstSetting.TickTime);
      count++;
      if (count >= (1000 / ConstSetting.TickTime) * 60)
      {
        GC.Collect(0);
      }
      ExecuteAsync(stoppingToken);
    }
  }
}
