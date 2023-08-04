using IdleWithBlazor.Common.Consts;
using IdleWithBlazor.Common.Enums;
using IdleWithBlazor.Common.Interfaces.Items;
using IdleWithBlazor.Model.GameItems.Templates.Equipments;
using IdleWithBlazor.Server.Services;

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
      while (!stoppingToken.IsCancellationRequested)
      {
        if (hubService == null)
        {
          using var scope = sp.CreateScope();
          hubService = scope.ServiceProvider.GetService<IHubServices>();
        }
        foreach (var user in (await hubService.ConnectedUsers()).Except(service.GetUserWithCharacters()))
        {
          await service.CreateCharacterAsync(user);
          await service.NewRoomAsync(user);
        }
        

        await service.OnTick(sp);
        await hubService.Broadcast(service.GetCharacters(), service.Games().Where(b => b.IsClosed != true));

        count++;
        if (count >= (1000 / ConstSetting.TickTime) * 60)
        {
          //GC.Collect(0);
          count = 0;
        }
#if RELEASE
        GC.Collect();
#endif
        await Task.Delay(ConstSetting.TickTime);
      }
    }
  }
}
