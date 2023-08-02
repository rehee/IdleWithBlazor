using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdleWithBlazor.Common.DTOs.Actors
{
  public class GameRoomDTO: ActorDTO
  {
    public CharacterDTO? Owner { get; set; }
    public IEnumerable<CharacterDTO>? Guest { get; set; }
    public GameMapDTO? GameMap { get; set; }
  }
}
