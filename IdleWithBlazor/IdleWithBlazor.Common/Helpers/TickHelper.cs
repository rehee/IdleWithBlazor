using IdleWithBlazor.Common.Consts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdleWithBlazor.Common.Helpers
{
  public static class TickHelper
  {
    public static int GetColdDownTick(decimal? attack, decimal? cd)
    {

      var attackTick = 0;
      if (attack.HasValue && attack > 0)
      {
        attackTick = (int)(1000 / attack / ConstSetting.TickTime);

      }
      var cdTick = 0;
      if (cd.HasValue && cd > 0)
      {
        cdTick = (int)(1000 * cd / ConstSetting.TickTime);
      }

      return attackTick + cdTick;
    }
  }
}
