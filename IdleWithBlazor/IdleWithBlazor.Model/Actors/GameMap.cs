using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdleWithBlazor.Model.Actors
{
  public class GameMap : Actor
  {

    public override IEnumerable<IActor> Actors
    {
      get
      {
        var players = Players != null ? Players : Enumerable.Empty<IActor>();
        var mobs = Monsters != null ? Monsters : Enumerable.Empty<IActor>();

        return players.Concat(mobs);
      }
      set => base.Actors = value;
    }
    public override async Task OnTick()
    {
      await base.OnTick();

    }
    public List<Player> Players { get; set; }
    public List<Monster> Monsters { get; set; }

  }
}
