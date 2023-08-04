using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdleWithBlazor.Common.DTOs.Actors
{
  public class GameMapDetailDTO : ActorDTO
  {
    public int MapLevel { get; set; }
    public int PackSize { get; set; }
  }
}
