using IdleWithBlazor.Common.DTOs;
using IdleWithBlazor.Common.Enums;
using IdleWithBlazor.Common.Helpers;
using IdleWithBlazor.Common.Interfaces.Actors;
using IdleWithBlazor.Common.Interfaces.GameActions;
using IdleWithBlazor.Common.Interfaces.Items;
using IdleWithBlazor.Model.Actions;
using IdleWithBlazor.Model.Actors;
using IdleWithBlazor.Model.Characters;
using IdleWithBlazor.Model.GameItems.Inventories;
using IdleWithBlazor.Model.GameItems.Templates.Equipments;
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
      ActorHelper.AddMapper<IInventory, Inventory>();
    }
    public static bool IsInit { get; set; }
    public static void InitModel()
    {
      if (IsInit)
      {
        return;
      }
      IsInit = true;
      ActorDTOHelper.InitDTOMapper();
      InitMapper();
      ActorHelper.UpdateActionPool(
       ActionsDtos.Select(b => new ActionSkill(b)).ToArray()
        );
    }

    public static ITemplate[] ITemplatePool = new ITemplate[]
    {
      new EquipmentTemplate(EnumEquipment.Body,"body_Aromor"),
      new EquipmentTemplate(EnumEquipment.OneHand,"On_Hand_Sword"),
      new EquipmentTemplate(EnumEquipment.Waist,"waist_Aromor"),
      new EquipmentTemplate(EnumEquipment.Foot,"foot_Aromor"),
    };

    public static ActionSkillInfoDTO[] ActionsDtos = new ActionSkillInfoDTO[]
    {
      new ActionSkillInfoDTO
      {
        Id = Guid.NewGuid(),
        Name = "连锁闪电",
        DamageRate=8,
        AttackSpeedRate = 1,
        TargetNumber=5,
        DamageChainReduction=0.75m,
        IsChain=true,
        ReChainTarget=false,
        CoolDown=3,
      },
      new ActionSkillInfoDTO
      {
        Id = Guid.NewGuid(),
        Name = "英勇打击",
        DamageRate=100m,
        AttackSpeedRate = 1,
        TargetNumber=5,
        CoolDown=3,
      },
    };
  }
}
