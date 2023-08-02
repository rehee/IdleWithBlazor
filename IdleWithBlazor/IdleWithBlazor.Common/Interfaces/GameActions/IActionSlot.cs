using IdleWithBlazor.Common.Interfaces.Actors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdleWithBlazor.Common.Interfaces.GameActions
{
  public interface IActionSlot : IActor
  {
    IActionSkill? ActionSkill { get; }
    int? CoolDownTickRemain { get; }
    int? CoolDownTick { get; }
    decimal AttackSpeed { get; }
    void UpdateActionSlot();
    void Init(ICharacter owner);
    void SelectSkill(IActionSkill? skill);
    bool? CurrentTick { get; }
  }
}
