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
      if (hubService == null)
      {
        using var scope = sp.CreateScope();
        hubService = scope.ServiceProvider.GetService<IHubServices>();
      }
      var connectedUsers = await hubService.ConnectedUsers();
      var currentGames = service.Games();
      var userWithNoGame = connectedUsers.Where(b => !currentGames.Select(b => b.OwnerId).Contains(b)).ToArray();
      await Task.WhenAll(userWithNoGame.Select(b => service.NewRoomAsync(b)));
      await service.OnTick(sp);
      

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
