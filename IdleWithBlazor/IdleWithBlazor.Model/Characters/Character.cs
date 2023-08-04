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
    public async void Init()
    {
      ActionSlots = new ConcurrentDictionary<int, IActionSlot>();
      for (var i = 0; i < 6; i++)
      {
        var skillSlot = ActorHelper.New<IActionSlot>();
        skillSlot.Init(this);
        ActionSlots.TryAdd(i, skillSlot);
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
      this.inventory = ActorHelper.New<IInventory>();
      this.inventory.Init(this);

      Equiptor = ActorHelper.New<IEquiptor>();
      Equiptor.Init(this);
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

    public Task<bool> PickSkill(Guid skillId, int slot)
    {
      if (ActionSlots.TryGetValue(slot, out var skillslot))
      {
        var skill = ActorHelper.ActionSkillPool.FirstOrDefault(b => b.Id == skillId);
        skillslot.SelectSkill(skill);
        skillslot.UpdateActionSlot();
      }
      return Task.FromResult(true);

    }

    public ConcurrentDictionary<int, IActionSlot>? ActionSlots { get; set; }

    public int CurrentExp { get; set; }
    public int NextLevelExp { get; set; }
    public bool EnableLevelUp { get; set; }
    public int Level { get; set; }

    public Task<bool> UpdatePlayerAsync()
    {
      ThisPlayer.MaxHp = 100 + 100 * (Level - 1);
      ThisPlayer.CurrentHp = ThisPlayer.MaxHp;
      ThisPlayer.NextLevelExp = NextLevelExp;
      ThisPlayer.CurrentExp = CurrentExp;
      ThisPlayer.Level = Level;
      ThisPlayer.MinAttack = BaseAttack;
      ThisPlayer.MaxAttack = BaseAttack;
      return Task.FromResult(true);
    }



    public Task<bool> GainCurrency(int exp)
    {
      ExpHelper.GainExp(exp, this);
      BaseAttack = 1 + Level;
      return UpdatePlayerAsync();
    }
    public IEquiptor Equiptor { get; set; }
    private IInventory inventory { get; set; }
    public IEnumerable<IGameItem>? Items()
    {
      foreach (var item in inventory.Items())
      {
        yield return item;
      }
    }

    public Task<bool> PickItemAsync(IGameItem? item)
    {
      return inventory.PickItemAsync(item);
    }

    public Task<IGameItem?> TakeOutItemAsync(Guid itemId)
    {
      return inventory.TakeOutItemAsync(itemId);
    }

    public Task<bool> DestoryItemAsync(Guid itemId)
    {
      return inventory.DestoryItemAsync(itemId);
    }
    public override void Dispose()
    {
      inventory.Dispose();
      inventory = null;
      Equiptor.Dispose();
      Equiptor = null;
      base.Dispose();
    }
  }
}
