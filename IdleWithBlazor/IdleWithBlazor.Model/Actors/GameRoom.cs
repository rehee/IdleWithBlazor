using IdleWithBlazor.Common.Interfaces.Actors;

namespace IdleWithBlazor.Model.Actors
{
  public class GameRoom : Actor
  {
    public override Type TypeDiscriminator => typeof(GameRoom);
    public override IEnumerable<IActor> Children
    {
      get
      {
        return Map != null ? new IActor[] { Map } : Enumerable.Empty<IActor>();
      }
      set => base.Children = value;
    }
    public GameMap Map { get; set; }
  }
}
