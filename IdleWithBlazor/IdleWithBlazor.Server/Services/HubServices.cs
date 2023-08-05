using IdleWithBlazor.Common.Consts;
using IdleWithBlazor.Common.DTOs.Actors;
using IdleWithBlazor.Common.DTOs.GameActions.Skills;
using IdleWithBlazor.Common.DTOs.Inventories;
using IdleWithBlazor.Common.Enums;
using IdleWithBlazor.Common.Helpers;
using IdleWithBlazor.Common.Interfaces.Actors;
using IdleWithBlazor.Common.Interfaces.Items;
using IdleWithBlazor.Common.Interfaces.Repostories;
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
    private readonly IHubConnectionRepostory repostory;
    private readonly IHubContext<MyHub> hubContext;
    private readonly IGameService gameService;
    public HubServices(IHubConnectionRepostory repostory, IHubContext<MyHub> hubContext, IGameService gameService)
    {
      this.repostory = repostory;
      this.hubContext = hubContext;
      this.gameService = gameService;
    }
    public async Task UserConnected(Guid userId, string connectionId)
    {
      _ = await repostory.AddConnectionIdAsync(connectionId, userId, EnumUserPage.Character);
      return;
    }
    public async Task UserLeave(string connectionId)
    {
      _ = await repostory.RemoveConnectionIdAsync(connectionId);
    }
    public async Task SetUserPage(string connectionId, EnumUserPage page)
    {
      _ = await repostory.UpdateConnectionPagesAsync(connectionId, page);
    }
    public async Task Broadcast(IEnumerable<ICharacter> characters, IEnumerable<IGameRoom> games)
    {
      IGameRoom[]? gameslist = null;
      var userLists = repostory.ConnectionUsers.ToArray();
      var broadCastQuery =
        from character in characters
        join user in repostory.ConnectionUsers on character.Id equals user.userId
        select (c: character, connection: user.connectionId, Client: hubContext.Clients.Client(user.connectionId), user: user.userId, Page: user.page);
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
              dto.Owner = q.c?.ToDTO<CharacterDTO>();
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
      broadCastQuery = null;
      await Task.CompletedTask;
    }


    bool IsDispose { get; set; }

    public Task<IEnumerable<Guid>> ConnectedUsers()
    {
      return Task.FromResult(repostory.Connectedusers as IEnumerable<Guid>);
    }

    public void Dispose()
    {
      if (IsDispose) return;
      IsDispose = true;
    }

    public ValueTask DisposeAsync()
    {
      Dispose();
      return ValueTask.CompletedTask;
    }

    #region equip or un-equip need refactory
    public async Task<bool> EquipOrUnequip(string connectionId, Guid? id, int? offset, EnumEquipmentSlot? slot)
    {
      var userId = await repostory.FindConnectedUser(connectionId);
      if (userId == null)
      {
        return false;
      }
      return await gameService.EquipOrUnequip(userId.Value, id, offset, slot);

    }

    public async Task SelectSkill(string connectionId, Guid skillId, int slot)
    {
      var userId = await repostory.FindConnectedUser(connectionId);
      if (userId == null)
      {
        return;
      }
      await gameService.SelectSkill(userId.Value, skillId, slot);

    }

    public async Task QuitGame(string connectionId)
    {
      var userId = await repostory.FindConnectedUser(connectionId);
      if (userId == null)
      {
        return;
      }
      await gameService.QuitGame(userId.Value);
    }

    public async Task CreateNewGame(string connectionId)
    {
      var userId = await repostory.FindConnectedUser(connectionId);
      if (userId == null)
      {
        return;
      }
      await gameService.NewRoomAsync(userId.Value);
    }
    public async Task JoinGame(string connectionId, Guid id)
    {
      var userId = await repostory.FindConnectedUser(connectionId);
      if (userId == null)
      {
        return;
      }
      await gameService.JoinGame(userId.Value, id);
    }
    #endregion
  }
}