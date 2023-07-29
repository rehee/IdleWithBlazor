using IdleWithBlazor.Common.Consts;
using IdleWithBlazor.Server.Services;

namespace IdleWithBlazor.Server.Tasks
{
  public class GameTask : BackgroundService
  {
    private readonly IGameService service;

    public GameTask(IGameService service)
    {
      this.service = service;
    }
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {

      await service.OnTick();
      await Task.Delay(ConstSetting.TickTime);
      ExecuteAsync(stoppingToken);
    }
  }
}
