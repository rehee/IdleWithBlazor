using IdleWithBlazor.Common.Helpers;
using IdleWithBlazor.Common.Interfaces.Actors;
using IdleWithBlazor.Common.Interfaces.GameActions;
using IdleWithBlazor.Model.Actors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdleWithBlazor.Model.Actions
{
  public class ActionSlot : Actor, IActionSlot
  {
    public override Type TypeDiscriminator => typeof(ActionSlot);

    public IActionSkill? ActionSkill { get; set; }

    public int? CoolDownTickRemain { get; set; }

    public int? CoolDownTick { get; set; }

    public decimal AttackSpeed { get; set; }

    public override void Init(IActor? parent, params object[] setInfo)
    {
      base.Init(parent, setInfo);

      var skill = setInfo.Where(b => b is IActionSkill).Select(b => b as IActionSkill).FirstOrDefault();
      if (skill != null)
      {
        SelectSkill(skill);
        UpdateActionSlot();
      }
    }

    public void SelectSkill(IActionSkill? skill)
    {
      this.Name = skill?.Name;
      ActionSkill = skill;
    }

    public void UpdateActionSlot()
    {
      if (ActionSkill == null)
      {
        AttackSpeed = 0;
        CoolDownTick = null;
        CoolDownTickRemain = null;
        return;
      }
      AttackSpeed = 1 * ActionSkill?.AttackSpeedRate ?? 0m;
      CoolDownTick = TickHelper.GetColdDownTick(AttackSpeed, ActionSkill?.CoolDown);
      CoolDownTickRemain = CoolDownTick;
    }
    public bool? CurrentTick { get; set; }
    public override Task<bool> OnTick(IServiceProvider sp)
    {
      CurrentTick = false;
      if (ActionSkill == null || !CoolDownTickRemain.HasValue || !CoolDownTick.HasValue)
      {
        return Task.FromResult(false);
      }
      if (CoolDownTickRemain <= 0)
      {
        CoolDownTickRemain = CoolDownTick;
        CurrentTick = true;
        return Task.FromResult(true);
      }
      CoolDownTickRemain--;
      return Task.FromResult(false);
    }

    public override void Dispose()
    {
      ActionSkill = null;
      base.Dispose();
    }
  }
}
