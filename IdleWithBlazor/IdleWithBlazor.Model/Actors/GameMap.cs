using IdleWithBlazor.Common.Interfaces.Actors;

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


  }
}
