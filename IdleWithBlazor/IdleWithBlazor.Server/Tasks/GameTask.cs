using IdleWithBlazor.Common.Consts;
using IdleWithBlazor.Common.Enums;
using IdleWithBlazor.Common.Helpers;
using IdleWithBlazor.Common.Interfaces.Items;
using IdleWithBlazor.Model.GameItems.Blurprints.Equipments;
using IdleWithBlazor.Model.GameItems.Items.Equipments;
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
    private readonly IBluePrintService bluePrintService;
    private readonly IItemService itemService;

    IHubServices? hubService { get; set; }
    public GameTask(IGameService service, IServiceProvider sp, IBluePrintService bluePrintService, IItemService itemService)
    {
      this.service = service;
      this.sp = sp;
      this.bluePrintService = bluePrintService;
      this.itemService = itemService;
      bluePrintService.AddBluePrint(new EquipmentBlueprint(EnumEquipment.OneHand, "匕首"));
      bluePrintService.AddBluePrint(new EquipmentBlueprint(EnumEquipment.TwoHands, "大斧头"));
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
      foreach (var g in service.Games())
      {
        Console.WriteLine("monseer is dead");
        
        var bp = bluePrintService.GetRandomBlueprint(null, null);
        var item = await itemService.GenerateItemAsync(bp);
        if (item is IEquipment ep)
        {
          await itemService.GenerateRandomProperty(ep);
        }
        Console.WriteLine(JsonSerializer.Serialize(item as Equipment, ConstSetting.Options));
        foreach (var m in g.Map.Monsters)
        {
          if (m.CurrentHp <= 0)
          {
            
          }
        }
      }

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
