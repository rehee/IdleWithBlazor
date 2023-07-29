using IdleWithBlazor.Common.Interfaces.Actors;
using IdleWithBlazor.Model.Actors;

namespace IdleWithBlazor.Model.Actions
{
  public class GameAction : Actor, IGameAction
  {
    public override Type TypeDiscriminator => throw new NotImplementedException();

  }
}
