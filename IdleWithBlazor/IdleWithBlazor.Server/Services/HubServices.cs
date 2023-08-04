using IdleWithBlazor.Common.Consts;
using IdleWithBlazor.Common.DTOs.Actors;
using IdleWithBlazor.Common.DTOs.GameActions.Skills;
using IdleWithBlazor.Common.DTOs.Inventories;
using IdleWithBlazor.Common.Enums;
using IdleWithBlazor.Common.Helpers;
using IdleWithBlazor.Common.Interfaces.Actors;
using IdleWithBlazor.Common.Interfaces.Items;
using IdleWithBlazor.Model.Actors;
using IdleWithBlazor.Server.Hubs;
using Microsoft.AspNetCore.SignalR;
using System.Collections.Concurrent;
using System.Runtime.CompilerServices;
using System.Text.Json;

namespace IdleWithBlazor.Server.Services
{
  public class HubServices : IHubServices
  {
    private readonly IHubContext<Hub> hubContext;
    private readonly IGameService gameService;
    static ConcurrentDictionary<string, Guid> ConnectionIdUserMap = new ConcurrentDictionary<string, Guid>();
    static ConcurrentDictionary<string, EnumUserPage> ConnectionIdUserPageMap = new ConcurrentDictionary<string, EnumUserPage>();
    public HubServices(IHubContext<Hub> hubContext, IGameService gameService)
    {
      this.hubContext = hubContext;
      this.gameService = gameService;
    }
    public Task UserConnected(Guid userId, string connectionId)
    {
      ConnectionIdUserMap.TryAdd(connectionId, userId);
      ConnectionIdUserPageMap.TryAdd(connectionId, EnumUserPage.Character);
      return Task.CompletedTask;
    }
    public Task UserLeave(string connectionId)
    {
      ConnectionIdUserMap.TryRemove(connectionId, out var id);
      ConnectionIdUserPageMap.TryRemove(connectionId, out var page);
      return Task.CompletedTask;
    }
    public Task SetUserPage(string connectionId, EnumUserPage page)
    {
      ConnectionIdUserPageMap.AddOrUpdate(connectionId, page, (k, v) => page);
      return Task.CompletedTask;
    }
    public async Task Broadcast(IEnumerable<ICharacter> characters, IEnumerable<IGameRoom> games)
    {
      IGameRoom[] gameslist = null;
      var connectionIdAndUser = ConnectionIdUserMap.Select(b => (connectionId: b.Key, userId: b.Value));
      var connectionGroup = ConnectionIdUserPageMap.Select(b => (id: b.Key, type: b.Value));
      var broadCastQuery =
        (from character in characters
         join user in connectionIdAndUser on character.Id equals user.userId
         join userPage in connectionGroup on user.connectionId equals userPage.id
         select (c: character, connection: user.connectionId, Client: hubContext.Clients.Client(user.connectionId), user: user.userId, Page: userPage.type));
      await Task.WhenAll(broadCastQuery.Select(q =>
      {
        switch (q.Page)
        {
          case EnumUserPage.Combat:
            if (q.c.Room == null)
            {
              return Task.CompletedTask;
            }
            string combatJson = null;
            try
            {
              var dto = q.c.Room.ToDTO<GameRoomDTO>();
              combatJson = JsonSerializer.Serialize(dto, ConstSetting.Options);
              dto = null;
            }
            catch (Exception ex)
            {
              Console.WriteLine(ex);
            }
            return q.Client.SendAsync("CombatMessage", combatJson);
          case EnumUserPage.Backpack:

            string inventoryJson = null;
            try
            {
              var dto = q.c.ToDTO<InventoryDTO>();
              inventoryJson = JsonHelper.ToJson(dto);
              dto = null;
            }
            catch (Exception ex)
            {
              Console.WriteLine(ex);
            }
            return q.Client.SendAsync("BackPackMessage", inventoryJson);
          case EnumUserPage.Character:
            string skilJson = null;
            try
            {
              var dto = q.c.ToDTO<SkillBookDTO>();
              skilJson = JsonHelper.ToJson(dto);
              dto = null;
            }
            catch (Exception ex)
            {
              Console.WriteLine(ex);
            }
            return q.Client.SendAsync("CharacterMessage", skilJson);
          case EnumUserPage.Map:
            if (q.c.Room?.Map == null)
            {
              return Task.CompletedTask;
            }
            string mapJson = null;
            try
            {
              mapJson = JsonHelper.ToJson(q.c.Room.Map.ToDTO<GameMapDetailDTO>());
            }
            catch (Exception mapJsonex)
            {

            }
            return q.Client.SendAsync("MapMessage", mapJson);
          case EnumUserPage.Miscell:
            var gameListDto = new GameListDTO();
            Guid currentGameId = Guid.Empty;
            if (q.c.Room != null)
            {
              currentGameId = q.c.Room.Id;
              gameListDto.CurrentGame = q.c.Room.ToDTO<GameListItemDTO>();
            }
            if (gameslist == null)
            {
              gameslist = games.ToArray();
            }
            gameListDto.Games = gameslist.Where(b => b.Id != currentGameId).Select(b => b.ToDTO<GameListItemDTO>()).ToArray();
            return q.Client.SendAsync("MiscellMessage", JsonHelper.ToJson(gameListDto));
          default:
            return Task.CompletedTask;
        }
      }));
      connectionIdAndUser = null;
      connectionGroup = null;
      broadCastQuery = null;
      await Task.CompletedTask;
    }


    bool IsDispose { get; set; }

    public Task<IEnumerable<Guid>> ConnectedUsers()
    {
      var result = ConnectionIdUserMap.Values.Select(b => b) ?? Enumerable.Empty<Guid>();
      return Task.FromResult(result);
    }

    public void Dispose()
    {
      if (IsDispose) return;
      IsDispose = true;
      GC.SuppressFinalize(this);
    }

    public ValueTask DisposeAsync()
    {
      Dispose();
      return ValueTask.CompletedTask;
    }

    #region equip or un-equip need refactory
    public async Task<bool> EquipOrUnequip(string connectionId, Guid? id, int? offset, EnumEquipmentSlot? slot)
    {
      var userIdFound = ConnectionIdUserMap.TryGetValue(connectionId, out var userId);
      if (!userIdFound || userId == null)
      {
        return false;
      }
      var users = GameService.GameRooms.Values.Where(b => b.OwnerId == userId).Select(b => b.GameOwner).FirstOrDefault();
      if (users == null)
      {
        return false;
      }
      if (slot.HasValue)
      {
        var unEquiped = users.Equiptor.UnEquip(slot.Value);
        foreach (var e in unEquiped)
        {
          await users.PickItemAsync(e);
        }
      }
      else if (id.HasValue)
      {
        var itemPick = await users.TakeOutItemAsync(id.Value);
        if (itemPick != null && itemPick is IEquipment equipPick)
        {
          IEnumerable<IEquipment?> takeoffs = Enumerable.Empty<IEquipment?>();
          switch (equipPick.EquipmentType)
          {
            case EnumEquipment.Head:
              takeoffs = users.Equiptor.UnEquip(EnumEquipmentSlot.Head);
              break;
            case EnumEquipment.Neck:
              takeoffs = users.Equiptor.UnEquip(EnumEquipmentSlot.Neck);
              break;
            case EnumEquipment.Shoulder:
              takeoffs = users.Equiptor.UnEquip(EnumEquipmentSlot.Shoulder);
              break;
            case EnumEquipment.Body:
              takeoffs = users.Equiptor.UnEquip(EnumEquipmentSlot.Body);
              break;
            case EnumEquipment.Hand:
              takeoffs = users.Equiptor.UnEquip(EnumEquipmentSlot.Hand);
              break;
            case EnumEquipment.Finger:
              if (offset == 1)
              {
                takeoffs = users.Equiptor.UnEquip(EnumEquipmentSlot.Rightinger);
              }
              else
              {
                takeoffs = users.Equiptor.UnEquip(EnumEquipmentSlot.LeftFinger);
              }

              break;
            case EnumEquipment.Waist:
              takeoffs = users.Equiptor.UnEquip(EnumEquipmentSlot.Waist);
              break;
            case EnumEquipment.Wrist:
              takeoffs = users.Equiptor.UnEquip(EnumEquipmentSlot.Wrist);
              break;
            case EnumEquipment.Leg:
              takeoffs = users.Equiptor.UnEquip(EnumEquipmentSlot.Leg);
              break;
            case EnumEquipment.Foot:
              takeoffs = users.Equiptor.UnEquip(EnumEquipmentSlot.Foot);
              break;
            case EnumEquipment.MainHand:
              takeoffs = users.Equiptor.UnEquip(EnumEquipmentSlot.MainHand);
              break;
            case EnumEquipment.OffHand:
              takeoffs = users.Equiptor.UnEquip(EnumEquipmentSlot.OffHand);
              break;
            case EnumEquipment.OneHand:
              if (offset == 1)
              {
                takeoffs = users.Equiptor.UnEquip(EnumEquipmentSlot.OffHand);
              }
              else
              {
                takeoffs = users.Equiptor.UnEquip(EnumEquipmentSlot.MainHand);
              }
              break;
            case EnumEquipment.TwoHands:
              takeoffs = users.Equiptor.UnEquip(EnumEquipmentSlot.OffHand, EnumEquipmentSlot.MainHand);
              break;
          }
          foreach (var item in takeoffs)
          {
            await users.PickItemAsync(item);
          }
          if (users.Equiptor.Equip(equipPick, offset))
          {
            await users.DestoryItemAsync(equipPick.Id);
          }

          takeoffs = null;
        }
      }
      return true;
    }

    public async Task SelectSkill(string connectionId, Guid skillId, int slot)
    {

      if (ConnectionIdUserMap.TryGetValue(connectionId, out var userId) && GameService.Characters.TryGetValue(userId, out var character))
      {
        await character.PickSkill(skillId, slot);
      }
    }

    public async Task QuitGame(string connectionId)
    {
      if (ConnectionIdUserMap.TryGetValue(connectionId, out var userId))
      {
        await gameService.QuitGame(userId);
      }
    }
    public async Task CreateNewGame(string connectionId)
    {
      if (ConnectionIdUserMap.TryGetValue(connectionId, out var userId))
      {
        await gameService.NewRoomAsync(userId);
      }
    }
    public async Task JoinGame(string connectionId, Guid id)
    {
      if (ConnectionIdUserMap.TryGetValue(connectionId, out var userId))
      {
        await gameService.JoinGame(userId, id);
      }
    }
    #endregion
  }
}