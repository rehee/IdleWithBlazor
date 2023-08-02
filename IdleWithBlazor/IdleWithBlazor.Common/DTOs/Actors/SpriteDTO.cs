using IdleWithBlazor.Common.Interfaces.Actors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace IdleWithBlazor.Common.DTOs.Actors
{
  public class SpriteDTO : ActorDTO, ILeveled
  {
    public BigInteger MaxHp { get; set; }
    public BigInteger CurrentHp { get; set; }
    public int Level { get; set; }
  }
}
