using IdleWithBlazor.Common.Helpers;
using IdleWithBlazor.Common.Interfaces.Actors;
using IdleWithBlazor.Model.Actors;
using IdleWithBlazor.Model.Characters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdleWithBlazor.Model.Helpers
{
  public static class ModelHelper
  {
    public static void InitModel()
    {
      ActorHelper.AddMapper<IGameRoom, GameRoom>();
      ActorHelper.AddMapper<IGameMap, GameMap>();
      ActorHelper.AddMapper<ICharacters, Character>();
      ActorHelper.AddMapper<IPlayer, Player>();
      ActorHelper.AddMapper<IMonster, Monster>();
      ActorHelper.AddMapper<IEquiptor, Equiptor>();
    }
  }
}
