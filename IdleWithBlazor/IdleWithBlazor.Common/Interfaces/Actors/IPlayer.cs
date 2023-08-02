using IdleWithBlazor.Common.Interfaces.GameActions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdleWithBlazor.Common.Interfaces.Actors
{
  public interface IPlayer : ISprite
  {
    void SetPlayerFromCharacter(ICharacters character);
   
  }
}
