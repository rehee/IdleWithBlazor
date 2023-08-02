using IdleWithBlazor.Common.Interfaces.GameActions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace IdleWithBlazor.Common.Interfaces.Actors
{
  public interface ISprite : IActor, ILeveled
  {
    BigInteger MaxHp { get; set; }
    BigInteger CurrentHp { get; set; }
    int MinAttack { get; set; }
    int MaxAttack { get; set; }
    IActionSlot? ActionSlots { get; }

    IActionSkill[]? ActionSkills { get; }
    void SetActions(IActionSkill[]? skills);
  }
}
