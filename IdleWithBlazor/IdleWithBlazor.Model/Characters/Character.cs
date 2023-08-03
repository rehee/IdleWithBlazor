using IdleWithBlazor.Common.Helpers;
using IdleWithBlazor.Common.Interfaces.Actors;
using IdleWithBlazor.Common.Interfaces.GameActions;
using IdleWithBlazor.Common.Interfaces.Items;
using IdleWithBlazor.Model.Actions;
using IdleWithBlazor.Model.Actors;
using System.Collections.Concurrent;

namespace IdleWithBlazor.Model.Characters
{
  public class Character : Actor, ICharacter
  {
    public override Type TypeDiscriminator => typeof(Character);

    public IGameRoom? Room { get; set; }
    IPlayer? _player { get; set; }
    public IPlayer ThisPlayer
    {
      get
      {
        if (_player == null)
        {
          Init();
        }
        return _player;
      }
    }
    public int BaseAttack { get; set; }
    public void Init()
    {
      ActionSlots = new ConcurrentDictionary<int, IActionSlot>();
      var actionIndex = 0;
      foreach (var skill in ActorHelper.ActionSkillPool)
      {
        var skillSlot = ActorHelper.New<IActionSlot>();
        skillSlot.Init(this);
        skillSlot.SelectSkill(skill);
        skillSlot.UpdateActionSlot();
        ActionSlots.TryAdd(actionIndex, skillSlot);
        actionIndex++;
      }
      Level = 1;
      CurrentExp = 0;
      NextLevelExp = ExpHelper.GetNextLevelExp(Level);
      EnableLevelUp = true;
      if (_player == null)
      {
        _player = ActorHelper.New<IPlayer>();
        _player.SetPlayerFromCharacter(this);
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

    public ConcurrentDictionary<int, IActionSlot>? ActionSlots { get; set; }

    public int CurrentExp { get; set; }
    public int NextLevelExp { get; set; }
    public bool EnableLevelUp { get; set; }
    public int Level { get; set; }

    public Task<bool> UpdatePlayerAsync()
    {
      ThisPlayer.MaxHp = ThisPlayer.MaxHp;
      ThisPlayer.NextLevelExp = NextLevelExp;
      ThisPlayer.CurrentExp = CurrentExp;
      ThisPlayer.Level = Level;
      ThisPlayer.MinAttack = BaseAttack;
      ThisPlayer.MaxAttack = BaseAttack;
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

    public Task<bool> GainCurrency(int exp)
    {
      ExpHelper.GainExp(exp, this);
      BaseAttack = 1 + Level;
      return UpdatePlayerAsync();
    }
  }
}
