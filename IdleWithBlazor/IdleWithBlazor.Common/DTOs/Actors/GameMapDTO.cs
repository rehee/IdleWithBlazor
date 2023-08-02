using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdleWithBlazor.Common.DTOs.Actors
{
  public class GameMapDTO: ActorDTO
  {
    public IEnumerable<PlayerDTO>? Players { get; set; }
    public IEnumerable<MonsterDTO>? Monsters { get; set; }
  }
}
