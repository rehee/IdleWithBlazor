﻿using IdleWithBlazor.Model.Actions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace IdleWithBlazor.Common.Interfaces.Actors
{
  public interface ISprite : IActor
  {
    BigInteger MaxHp { get; set; }
    BigInteger CurrentHp { get; set; }
    IActionSkill[]? ActionSkills { get; }
    void SetActions(IActionSkill[]? skills);
  }
}
