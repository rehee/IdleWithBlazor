using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdleWithBlazor.Common.Interfaces.GameActions
{
  public interface IActionSkillInfo : IName, IGuidId
  {
    decimal DamageRate { get; }
    decimal CoolDown { get; }
    bool GlobalCoolDown { get; }
    decimal AttackSpeedRate { get; }
    int TargetNumber { get; }
    bool IsChain { get; }
    bool ReChainTarget { get; }
    decimal DamageChainReduction { get; }
  }
}
