using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace IdleWithBlazor.Common.DTOs.Actors
{
  public class SpriteDTO : ActorDTO
  {
    public BigInteger MaxHp { get; set; }
    public BigInteger CurrentHp { get; set; }
  }
}
