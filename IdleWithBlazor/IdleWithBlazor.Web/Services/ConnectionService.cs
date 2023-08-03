﻿using IdleWithBlazor.Common.Consts;
using IdleWithBlazor.Common.DTOs.Actors;
using IdleWithBlazor.Common.DTOs.Inventories;
using IdleWithBlazor.Common.Enums;
using IdleWithBlazor.Common.Helpers;
using IdleWithBlazor.Common.Services;
using IdleWithBlazor.Web.Models;
using Microsoft.AspNetCore.SignalR.Client;
using System.Text.Json;

namespace IdleWithBlazor.Web.Services
{
  public class ConnectionService : IConnection
  {
    private readonly IStorageService storage;
    private readonly IScopedContext<GameRoomDTO> room;
    private readonly IScopedContext<InventoryDTO> inventory;
    private readonly Setting setting;

    private HubConnection? hub { get; set; } = null;
    public ConnectionService(IStorageService storage, IScopedContext<GameRoomDTO> room, IScopedContext<InventoryDTO> inventory, Setting setting)
    {
      this.storage = storage;
      this.room = room;
      this.inventory = inventory;
      this.setting = setting;
    }
    public async Task<bool> AbortAsync()
    {
      if (hub != null)
      {
        try
        {
          await hub.DisposeAsync();
        }
        catch
        {

        }
      }
      return true;
    }
    public async Task<bool> ConnectionAsync()
    {
      if (hub != null)
      {
        return true;
      }
      var token = await storage.GetAsync<string?>(ConstKey.UserAccessToken);
      if (string.IsNullOrEmpty(token))
      {
        return false;
      }
      hub = new HubConnectionBuilder()
       .WithUrl($"{setting.ServerHost}/myhub", options =>
       {
         options.AccessTokenProvider = () => Task.FromResult<string?>(token);
       })

       .Build();
      try
      {
        hub.On<string, string>("ReceiveMessage", (a, b) =>
        {
          Console.WriteLine(b);
        });
        hub.On<string>("CombatMessage", r =>
        {
          var obj = JsonHelper.ToObject<GameRoomDTO>(r);
          room.SetValue(obj);
        });
        hub.On<string>("BackPackMessage", r =>
        {
          inventory.SetValue(JsonHelper.ToObject<InventoryDTO>(r));
        });
        await hub.StartAsync();
        return true;
      }
      catch
      {
        return false;
      }

    }

    public async Task Send()
    {
      await hub.SendAsync("SendMessage", "111", "111");
    }

    public Task KeepSend()
    {
      return Task.CompletedTask;
    }
    public async Task SetPage(EnumUserPage page)
    {
      if (hub == null)
      {
        await ConnectionAsync();
      }
      await hub.SendAsync("SetUserPage", page);
    }

    public async Task<bool> EquipItem(Guid? id, int? offset)
    {
      await hub.SendAsync("EquipItem", id, offset);
      return true;
    }

    public async Task<bool> UnEquipItem(EnumEquipmentSlot slot)
    {
      await hub.SendAsync("UnEquipItem", slot);
      return true;
    }
  }
}
