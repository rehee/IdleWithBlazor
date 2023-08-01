using IdleWithBlazor.Common.Interfaces.Actors;
using IdleWithBlazor.Common.Interfaces.Items;

namespace IdleWithBlazor.Model.Actors
{
  public class GameMap : Actor
  {
    public override Type TypeDiscriminator => typeof(GameMap);
    public override IEnumerable<IActor> Children
    {
      get
      {
        var players = Players != null ? Players : Enumerable.Empty<IActor>();
        var mobs = Monsters != null ? Monsters : Enumerable.Empty<IActor>();

        return players.Concat(mobs);
      }
      set => base.Children = value;
    }
    public List<Player> Players { get; set; }
    public List<Monster> Monsters { get; set; }

    public void Add<T>(T item) where T : class
    {
      if (item is Player p)
      {
        lock (this)
        {
          if (Players == null)
          {
            Players = new List<Player>();
          }
          Players.Add(p);
        }
      }
      if (item is Monster m)
      {
        lock (this)
        {
          if (Monsters == null)
          {
            Monsters = new List<Monster>();
          }
          Monsters.Add(m);
        }
      }
    }

    
  }
}
