using IdleWithBlazor.Common.Interfaces.Actors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdleWithBlazor.Model.Actors
{
  public class Monster : Sprite, IMonster
  {
    public override Type TypeDiscriminator => typeof(Monster);

  }
}
