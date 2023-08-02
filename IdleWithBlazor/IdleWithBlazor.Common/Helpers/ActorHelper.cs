using IdleWithBlazor.Common.DTOs.Actors;
using IdleWithBlazor.Common.Interfaces.Actors;
using IdleWithBlazor.Common.Interfaces.GameActions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdleWithBlazor.Common.Helpers
{
  public static class ActorHelper
  {
    private static Dictionary<Type, Type> mapper = new Dictionary<Type, Type>();
    public static void AddMapper<T, K>() where T : IActor where K : T, new()
    {
      mapper.TryAdd(typeof(T), typeof(K));
    }

    public static T New<T>(Guid? id = null, string? name = null) where T : IActor
    {
      if (mapper.TryGetValue(typeof(T), out var type) && type != null)
      {
        var obj = Activator.CreateInstance(type);
        if (obj is T tObj)
        {
          id = id ?? Guid.NewGuid();
          tObj.Id = id.Value;
          tObj.Name = name ?? id.Value.ToString();
          return tObj;
        }
      }
      return default(T?);
    }

    private static List<IActionSkill> actionSkillPool = new List<IActionSkill>();
    public static void UpdateActionPool(params IActionSkill[] actions)
    {
      actionSkillPool.AddRange(actions);
    }
    public static IEnumerable<IActionSkill> ActionSkillPool => actionSkillPool;

    public static PlayerDTO ToDTO(this IPlayer player)
    {
      return new PlayerDTO
      {
        Id = player.Id,
        Name = player.Name,
        CurrentHp = player.CurrentHp,
        MaxHp = player.MaxHp,
      };
    }
    public static MonsterDTO ToDTO(this IMonster player)
    {
      return new MonsterDTO
      {
        Id = player.Id,
        Name = player.Name,
        CurrentHp = player.CurrentHp,
        MaxHp = player.MaxHp,
      };
    }
    public static CharacterDTO ToDTO(this ICharacter character)
    {
      return new CharacterDTO
      {
        Id = character.Id,
        Name = character.Name,
      };
    }
    public static GameMapDTO ToDTO(this IGameMap gameMap)
    {
      return new GameMapDTO
      {
        Id = gameMap.Id,
        Name = gameMap.Name,
        Players = gameMap.Players?.Select(b => b.ToDTO()),
        Monsters = gameMap.Monsters?.Select(b => b.ToDTO()),
      };
    }
    public static GameRoomDTO ToDTO(this IGameRoom room)
    {
      return new GameRoomDTO
      {
        Id = room.Id,
        Name = room.Name,
        Owner = room.GameOwner?.ToDTO(),
        Guest = room.Guests?.Select(x => x.ToDTO()),
        GameMap = room.Map?.ToDTO()
      };
    }
  }
}
