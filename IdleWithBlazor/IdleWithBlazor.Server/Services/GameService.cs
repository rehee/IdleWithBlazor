using IdleWithBlazor.Model.Actions;
using IdleWithBlazor.Model.Actors;
using IdleWithBlazor.Model.GameItems.Items.Equipments;
using System.Collections.Concurrent;

namespace IdleWithBlazor.Server.Services
{
  public class GameService : IGameService
  {


    public static ConcurrentDictionary<Guid, GameRoom> GameRooms { get; set; } = new ConcurrentDictionary<Guid, GameRoom>();

    public IEnumerable<GameRoom> Games()
    {
      return GameRooms.Values.ToArray();
    }

    public Task<GameRoom> GetUserRoomAsync(Guid userId)
    {
      if (GameRooms.TryGetValue(userId, out var room))
      {
        return Task.FromResult(room);
      }
      return Task.FromResult(default(GameRoom));
    }

    public Task NewRoomAsync(Guid userId)
    {
      var game = new GameRoom();
      game.OwnerId = userId;
      game.Map = new GameMap();
      var player = new Player();
      var mob = new Monster();
      game.Map.Add(player);
      game.Map.Add(mob);
      mob.MaxHp = 50;
      mob.CurrentHp = 50;
      var skill = new Attack();
      skill.Init(1);
      player.SetAction(skill);
      player.SetTarget(mob);
      GameRooms.TryAdd(userId, game);
      player.PickItem(new Equipment()
      {
        Name = "item",
        EquipmentType = Common.Enums.EnumEquipment.Neck
      });
      return Task.CompletedTask;
    }

    public async Task OnTick(IServiceProvider sp)
    {
      await Task.WhenAll(Games().Select(b => b.OnTick(sp)));
    }
  }
}
