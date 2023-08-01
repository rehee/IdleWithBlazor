using IdleWithBlazor.Common.Enums;
using IdleWithBlazor.Common.Helpers;
using IdleWithBlazor.Common.Interfaces.Actors;
using IdleWithBlazor.Common.Interfaces.Items;
using IdleWithBlazor.Model.Actors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace IdleWithBlazor.Model.Characters
{
  public class Character : Actor, ICharacters
  {
    public override Type TypeDiscriminator => typeof(Character);

    public IGameRoom? Room { get; protected set; }
    IPlayer? _player { get; set; }
    public IPlayer ThisPlayer
    {
      get
      {
        if (_player == null)
        {
          _player = ActorHelper.New<IPlayer>();
          _player.SetPlayerFromCharacter(this);
        }
        return _player;
      }
    }

    public async Task<IGameRoom?> CreateRoomAsync()
    {
      if (Room != null)
      {
        await Room.CloseGameAsync();
      }
      Room = ActorHelper.New<IGameRoom>();
      if (Room != null)
      {
        await Room.InitAsync(this);
      }
      return Room;
    }

    public async Task<bool> JoinGameAsync(IGameRoom game)
    {
      try
      {
        if (await game.JoinGameAsync(this))
        {
          Room = game;
        }
        return true;
      }
      catch
      {
        return false;
      }
    }

    public async Task<bool> KickPlayerAsync(Guid playerId)
    {
      if (Room == null || Room.GameOwner?.Id != this.Id)
      {
        return false;
      }
      return await Room.KickGuestAsync(playerId);
    }

    public async Task<bool> LeaveGameAsync()
    {
      if (Room == null)
      {
        return false;
      }
      var result = false;
      if (Room.GameOwner?.Id == this.Id)
      {
        result = await Room.CloseGameAsync();
      }
      else
      {
        result = await Room.KickGuestAsync(this.Id);
      }
      Room = null;
      return result;
    }

    public Equiptor Inventory { get; set; }

    public Task<bool> UpdatePlayerAsync()
    {
      ThisPlayer.MaxHp = 10;
      ThisPlayer.CurrentHp = ThisPlayer.MaxHp;
      
      return Task.FromResult(true);
    }

    public void PickItem(IGameItem item)
    {
      if (Inventory == null)
      {
        Inventory = new Equiptor();
      }
      if (item is IEquipment ep)
      {
        Inventory.Equip(ep);
      }



    }


  }
}
