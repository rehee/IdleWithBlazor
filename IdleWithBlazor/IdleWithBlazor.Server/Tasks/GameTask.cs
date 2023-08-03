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
    private readonly ITemplateService TemplateService;
    private readonly IItemService itemService;

    IHubServices? hubService { get; set; }
    public GameTask(IGameService service, IServiceProvider sp, ITemplateService TemplateService, IItemService itemService)
    {
      this.service = service;
      this.sp = sp;
      this.TemplateService = TemplateService;
      this.itemService = itemService;
      TemplateService.AddTemplate(new EquipmentTemplate(EnumEquipment.OneHand, "匕首"));
      TemplateService.AddTemplate(new EquipmentTemplate(EnumEquipment.TwoHands, "大斧头"));
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
        var connectedUsers = await hubService.ConnectedUsers();
        var currentGames = service.Games().Select(b => b.OwnerId).ToArray();
        var userWithNoGame = connectedUsers.Where(b => !currentGames.Contains(b)).ToArray();
        await Task.WhenAll(userWithNoGame.Select(b => service.NewRoomAsync(b)));
        await service.OnTick(sp);
        var games = service.Games();
        await hubService.Broadcast(games);
        currentGames = null;
        connectedUsers = null;
        userWithNoGame = null;
        games = null;
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
