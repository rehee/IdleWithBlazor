using IdleWithBlazor.Common.Consts;
using IdleWithBlazor.Common.DTOs.Actors;
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
    static ConcurrentDictionary<string, Guid> ConnectionIdUserMap = new ConcurrentDictionary<string, Guid>();
    static ConcurrentDictionary<string, EnumUserPage> ConnectionIdUserPageMap = new ConcurrentDictionary<string, EnumUserPage>();
    public HubServices(IHubContext<Hub> hubContext)
    {
      this.hubContext = hubContext;
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
    public async Task Broadcast(IEnumerable<IGameRoom> games)
    {
      var users = ConnectionIdUserMap
        .Select(b =>
        (
          connectionId: b.Key,
          userId: b.Value,
          game: games.FirstOrDefault(g => g.OwnerId.Equals(b.Value)))
        ).ToArray();
      var connectionGroup = ConnectionIdUserPageMap.Select(b => (id: b.Key, type: b.Value));
      var query =
        from c in connectionGroup
        join u in users on c.id equals u.connectionId into ug
        from user in ug.DefaultIfEmpty()
        select
        new
        {
          UserId = user.userId,
          ConnectionId = user.connectionId,
          Geme = user.game,
          Page = c.type,
          Client = hubContext.Clients.Client(user.connectionId)
        };

      await Task.WhenAll(query.Select(q =>
      {
        switch (q.Page)
        {
          case EnumUserPage.Combat:
            string combatJson = null;
            try
            {
              var dto = q.Geme.ToDTO<GameRoomDTO>();
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
              var dto = q.Geme.GameOwner.ToDTO<InventoryDTO>();
              inventoryJson = JsonHelper.ToJson(dto);
              dto = null;
            }
            catch (Exception ex)
            {
              Console.WriteLine(ex);
            }
            return q.Client.SendAsync("BackPackMessage", inventoryJson);

          default:
            return Task.CompletedTask;
        }
      }));
      query = null;
      await Task.CompletedTask;
    }


    bool IsDispose { get; set; }

    public Task<IEnumerable<Guid>> ConnectedUsers()
    {
      var result = ConnectionIdUserMap.Values.Select(b => b).ToArray() ?? Enumerable.Empty<Guid>();
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
  }
}
