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
    private readonly IHubContext<MyHub> hubContext;

    public GameTask(IGameService service, IServiceProvider sp, IHubContext<MyHub> hubContext)
    {
      this.service = service;
      this.sp = sp;
      this.hubContext = hubContext;
    }
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
      await service.OnTick();
      await hubContext.Clients.All.SendAsync("RoomMessage", JsonSerializer.Serialize(GameService.Room, JsonSerializerOptionsHelper.Default));
      await Task.Delay(ConstSetting.TickTime);
      ExecuteAsync(stoppingToken);
    }
  }
}
