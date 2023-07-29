using IdleWithBlazor.Model.Actors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdleWithBlazor.Model.Actions
{
  public class GameAction : IGameAction
  {
    protected virtual IActor Actor { get; set; }
    public virtual void SetActor(IActor actor)
    {
      Actor = actor;
      
    }
  }
}
