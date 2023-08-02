using IdleWithBlazor.Common.DTOs;
using IdleWithBlazor.Common.Helpers;
using IdleWithBlazor.Common.Interfaces.Actors;
using IdleWithBlazor.Common.Interfaces.GameActions;
using IdleWithBlazor.Model.Actions;
using IdleWithBlazor.Model.Actors;
using IdleWithBlazor.Model.Characters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdleWithBlazor.Model.Helpers
{
  public static class ModelHelper
  {
    public static void InitMapper()
    {
      ActorHelper.AddMapper<IGameRoom, GameRoom>();
      ActorHelper.AddMapper<IGameMap, GameMap>();
      ActorHelper.AddMapper<ICharacter, Character>();
      ActorHelper.AddMapper<IPlayer, Player>();
      ActorHelper.AddMapper<IMonster, Monster>();
      ActorHelper.AddMapper<IEquiptor, Equiptor>();
      ActorHelper.AddMapper<IActionSkill, ActionSkill>();
      ActorHelper.AddMapper<IActionSlot, ActionSlot>();
    }
    public static void InitModel()
    {
      InitMapper();
      ActorHelper.UpdateActionPool(
       ActionsDtos.Select(b => new ActionSkill(b)).ToArray()
        );
    }

    public static ActionSkillInfoDTO[] ActionsDtos = new ActionSkillInfoDTO[]
    {
      new ActionSkillInfoDTO
      {
        Id = Guid.NewGuid(),
        Name = "攻击",
        DamageRate=1,
        AttackSpeedRate = 1,
        TargetNumber=1,
        CoolDown=0,
      },
      new ActionSkillInfoDTO
      {
        Id = Guid.NewGuid(),
        Name = "英勇打击",
        DamageRate=1.5m,
        AttackSpeedRate = 1,
        TargetNumber=1,
        CoolDown=3,
      },
    };
  }
}
