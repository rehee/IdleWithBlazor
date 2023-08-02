using IdleWithBlazor.Common.Interfaces.Actors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace IdleWithBlazor.Common.Helpers
{
  public static class ExpHelper
  {
    public static int GetNextLevelExp(int level)
    {
      var levelMax = 100;
      int baseExp = 150;
      double expGrowthRate = 1.05;

      double expGrowthRate2 = 1.1;
      if (level < levelMax)
      {
        return (int)(baseExp * Math.Pow(expGrowthRate, level));
      }
      else
      {
        return (int)(baseExp * Math.Pow(expGrowthRate2, level));
      }
    }
    public static int GetMonsterExp(int level)
    {
      int baseExp = 10;
      int baseIncrease = 5;
      return (level - 1) * baseIncrease + baseExp;
    }

    public static void GainExp(int exp, ILevelupable levelUp)
    {
      levelUp.CurrentExp = levelUp.CurrentExp + exp;
      if (levelUp.CurrentExp >= levelUp.NextLevelExp)
      {
        if (levelUp.EnableLevelUp)
        {
          var nextXp = levelUp.CurrentExp - levelUp.NextLevelExp;
          levelUp.CurrentExp = 0;
          levelUp.Level++;
          GainExp(nextXp, levelUp);
        }
        else
        {
          levelUp.CurrentExp = levelUp.NextLevelExp;
          return;
        }
      }
    }
  }
}
