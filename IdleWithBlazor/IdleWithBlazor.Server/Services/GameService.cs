using IdleWithBlazor.Common.Enums;
using IdleWithBlazor.Common.Helpers;
using IdleWithBlazor.Common.Interfaces.Actors;
using IdleWithBlazor.Common.Interfaces.Items;
using IdleWithBlazor.Common.Interfaces.Repostories;

namespace IdleWithBlazor.Server.Services
{
  public class GameService : IGameService
  {
    private readonly IGameRepostory repostory;

    public GameService(IGameRepostory repostory)
    {
      this.repostory = repostory;
    }

    public IEnumerable<IGameRoom> Games()
    {
      foreach (var room in repostory.Roomes)
      {
        yield return room;
      }
    }
    public IEnumerable<ICharacter> GetCharacters()
    {
      foreach (var character in repostory.Characters)
      {
        yield return character;
      }
    }
    public IEnumerable<Guid> GetUserWithCharacters()
    {
      foreach (var userId in repostory.CharacterIds)
      {
        yield return userId;
      }
    }
    public Task<ICharacter?> GetCharacterAsync(Guid userId)
    {
      return repostory.FindCharacterAsync(userId);
    }
    public async Task<ICharacter?> CreateCharacterAsync(Guid userId)
    {
      var character = ActorHelper.New<ICharacter>(userId, $"查内姆 {userId.ToString().Split("-")[0]}");
      character.Init();

      if (await repostory.AddCharacterAsync(character))
      {
        return character;
      }
      return null;
    }
    public async Task<IGameRoom?> GetUserRoomAsync(Guid userId)
    {
      var user = await GetCharacterAsync(userId);
      return user?.Room;
    }

    public async Task NewRoomAsync(Guid userId)
    {
      var character = await GetCharacterAsync(userId);
      if (character == null)
      {
        character = await CreateCharacterAsync(userId);
      }
      if (character != null && character.Room != null)
      {
        return;
      }
      var game = await character.CreateRoomAsync();
      _ = await game.CreateMapAsync();
      _ = await game.Map.GenerateMobsAsync();
      _ = await repostory.AddRoomAsync(game);
    }


    public async Task QuitGame(Guid userId)
    {
      var character = await GetCharacterAsync(userId);
      if (character == null || character.Room == null)
      {
        return;
      }
      await character.LeaveGameAsync();

    }
    public async Task JoinGame(Guid userId, Guid id)
    {
      var character = await GetCharacterAsync(userId);
      if (character == null || character.Room != null)
      {
        return;
      }
      var gameRoom = repostory.Roomes.Where(b => b.IsClosed != true && b.Id == id).FirstOrDefault();
      if (gameRoom != null)
      {
        {
          await character.JoinGameAsync(gameRoom);
        }

      }
    }

    public async Task OnTick(IServiceProvider sp)
    {
      var closedGame = Games().Where(b => b.IsClosed).Select(b => b.Id).ToArray();
      foreach (var game in closedGame)
      {
        await repostory.RemoveRoomAsync(game);
      }
      closedGame = null;
      await Task.WhenAll(Games().Where(b => !b.IsClosed).Select(b => b.OnTick(sp)));
    }

    public async Task<bool> EquipOrUnequip(Guid userId, Guid? itemId, int? offset, EnumEquipmentSlot? slot)
    {
      var users = await repostory.FindCharacterAsync(userId);
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
      else if (itemId.HasValue)
      {
        var itemPick = await users.TakeOutItemAsync(itemId.Value);
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

    public async Task SelectSkill(Guid userId, Guid skillId, int slot)
    {
      var character = await repostory.FindCharacterAsync(userId);
      if (character == null)
      {
        return;
      }
      await character.PickSkill(skillId, slot);
    }


  }
}
