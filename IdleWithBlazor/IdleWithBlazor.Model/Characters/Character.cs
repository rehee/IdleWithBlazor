using IdleWithBlazor.Common.Helpers;
using IdleWithBlazor.Common.Interfaces.Actors;
using IdleWithBlazor.Common.Interfaces.GameActions;
using IdleWithBlazor.Common.Interfaces.Items;
using IdleWithBlazor.Model.Actors;

namespace IdleWithBlazor.Model.Characters
{
  public class Character : Actor, ICharacter
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

    public void Init()
    {
      ActionSlots = ActorHelper.New<IActionSlot>();
      ActionSlots.Init(this);
      ActionSlots.SelectSkill(ActorHelper.ActionSkillPool.FirstOrDefault());
      ActionSlots.UpdateActionSlot();
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

    public IActionSlot ActionSlots { get; set; }

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
