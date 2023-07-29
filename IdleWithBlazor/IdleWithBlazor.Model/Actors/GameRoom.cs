using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdleWithBlazor.Model.Actors
{
  public class GameRoom : Actor
  {
    public override IEnumerable<IActor> Actors
    {
      get
      {
        return Map != null ? new IActor[] { Map } : Enumerable.Empty<IActor>();
      }
      set => base.Actors = value;
    }
    public GameMap Map { get; set; }
  }
}
