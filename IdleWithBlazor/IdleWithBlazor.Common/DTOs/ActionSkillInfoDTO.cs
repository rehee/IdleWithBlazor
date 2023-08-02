using IdleWithBlazor.Common.Interfaces.GameActions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdleWithBlazor.Common.DTOs
{
  public class ActionSkillInfoDTO : IActionSkillInfo
  {
    public Guid Id { get; set; }
    public string? Name { get; set; }
    public decimal DamageRate { get; set; }

    public decimal CoolDown { get; set; }

    public bool GlobalCoolDown { get; set; }

    public decimal AttackSpeedRate { get; set; }

    public int TargetNumber { get; set; }

    public bool IsChain { get; set; }

    public bool ReChainTarget { get; set; }

    public decimal DamageChainReduction { get; set; }


  }
}
