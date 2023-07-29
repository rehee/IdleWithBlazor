using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdleWithBlazor.Model.Actors
{
  public class Player : Sprite
  {
    public override async Task OnTick()
    {
      await base.OnTick();
      Console.WriteLine("player tick");
    }
  }
}
