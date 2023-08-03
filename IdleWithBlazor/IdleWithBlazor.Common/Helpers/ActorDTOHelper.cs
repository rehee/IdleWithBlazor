using IdleWithBlazor.Common.DTOs.Actors;
using IdleWithBlazor.Common.DTOs.Inventories;
using IdleWithBlazor.Common.Enums;
using IdleWithBlazor.Common.Interfaces.Actors;
using IdleWithBlazor.Common.Interfaces.GameActions;
using IdleWithBlazor.Common.Interfaces.Items;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdleWithBlazor.Common.Helpers
{
  public static class ActorDTOHelper
  {
    public static T? ToDTO<T>(this IActor? input) where T : IActorDTO, new()
    {
      if (input == null)
      {
        return default(T?);
      }
      var dto = new T();
      SetDTO(input, dto);
      return dto;
    }

    public static void SetDTO(IActor input, IActorDTO? dto)
    {
      if (dto == null)
      {
        return;
      }
      dto.Name = input.Name;
      dto.Id = input.Id;
      foreach (var value in DTOSetMapper.Values)
      {
        value(input, dto);
      }

    }

    private static ConcurrentDictionary<Type, Action<IActor, IActorDTO>> DTOSetMapper = new ConcurrentDictionary<Type, Action<IActor, IActorDTO>>();
    public static void AddDTOMapper<K, T>(Action<K, T> mapperAdding) where T : IActorDTO, new() where K : IActor
    {
      DTOSetMapper.TryAdd(typeof(T), (actor, dto) =>
      {
        if (actor == null || dto == null)
        {
          return;
        }
        if (actor is K kActor && dto is T tDTO)
        {
          mapperAdding(kActor, tDTO);
        }
      });
    }

    public static void InitDTOMapper()
    {
      AddDTOMapper<ISprite, SpriteDTO>((input, dto) =>
      {
        dto.MaxHp = input.MaxHp;
        dto.CurrentHp = input.CurrentHp;
        dto.Level = input.Level;
      });
      AddDTOMapper<IPlayer, PlayerDTO>((input, dto) =>
      {
        dto.CurrentExp = input.CurrentExp;
      });

      AddDTOMapper<ICharacter, CharacterDTO>((input, dto) =>
      {
        dto.SkillSlots = input.ActionSlots?.OrderBy(b => b.Key).Select(b => b.Value.ToDTO<SkillSlotDTO>());
      });
      AddDTOMapper<IActionSlot, SkillSlotDTO>((input, dto) =>
      {
        dto.Name = input.Name;
        if (input.CoolDownTick.HasValue && input.CoolDownTick != 0)
        {
          dto.Processing = ((input.CoolDownTick - input.CoolDownTickRemain) * 100) / input.CoolDownTick;
        }
      });
      AddDTOMapper<ICharacter, CharacterDetailDTO>((input, dto) =>
      {
        dto.GameSummary = input.Room.ToDTO<GameSummaryDTO>();
      });
      AddDTOMapper<IGameMap, GameMapDTO>((input, dto) =>
      {
        dto.Players = input.Players().Select(b => b.ToDTO<PlayerDTO>());
        dto.Monsters = input.Monsters?.Select(b => b.ToDTO<MonsterDTO>());
      });
      AddDTOMapper<IGameRoom, GameRoomDTO>((input, dto) =>
      {
        dto.Owner = input.GameOwner?.ToDTO<CharacterDTO>();
        dto.Guests = input.Guests()?.Select(x => x.ToDTO<CharacterDTO>());
        dto.GameMap = input.Map?.ToDTO<GameMapDTO>();
      });
      AddDTOMapper<IGameRoom, GameSummaryDTO>((input, dto) =>
      {
        dto.Guests = input.Guests().Select(x => x.ToDTO<CharacterDTO>()) ?? null;
      });
      AddDTOMapper<ICharacter, InventoryDTO>((input, dto) =>
      {
        dto.Items = input.Items().Select(x => x.ToDTO<GameItemDTO>()).ToArray();
        dto.Equiptor = input.ToDTO<EquiptorDTO>();
      });
      AddDTOMapper<IGameItem, GameItemDTO>((input, dto) =>
      {
        dto.Type = input.ItemType;
        if (input is IEquipment equipment)
        {
          dto.EquipType = equipment.EquipmentType;
        }
      });
      AddDTOMapper<ICharacter, EquiptorDTO>((input, dto) =>
      {
        dto.Equpments = new Dictionary<Enums.EnumEquipmentSlot, EquipmentDTO>();
        var itemsEquiped = input.Equiptor.Equipments().ToArray();
        foreach (EnumEquipmentSlot value in Enum.GetValues(typeof(EnumEquipmentSlot)))
        {
          dto.Equpments.TryAdd(
            value,
            itemsEquiped.Where(b => b.slot == value && b.equipment != null).Select(b => new EquipmentDTO
            {
              Id = b.equipment.Id,
              Name = b.equipment.Name,
              EquipType = b.equipment.EquipmentType,
              Type = EnumItemType.Equipment,
            }).FirstOrDefault());
        }
        itemsEquiped = null;
      });

    }



  }
}
