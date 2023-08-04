using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdleWithBlazor.Common.DTOs.Actors
{
  public class GameListDTO : ActorDTO
  {
    public GameListItemDTO CurrentGame { get; set; }
    public GameListItemDTO[] Games { get; set; }
  }

  public class GameListItemDTO : ActorDTO
  {
    public string OwnerName { get; set; }
    public int PlayerNumber { get; set; }
  }
}
