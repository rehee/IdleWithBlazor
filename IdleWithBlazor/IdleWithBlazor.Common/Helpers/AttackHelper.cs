using IdleWithBlazor.Common.Interfaces.Actors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace IdleWithBlazor.Common.Helpers
{
  public static class AttackHelper
  {
    public static void ChainDamage(int chainRemain, BigInteger damage, int damageReduction, bool targetChained, List<ISprite> targets)
    {
      if (chainRemain <= 0 || targets?.Any() != true)
      {
        return;
      }

      var list = targets.Where(b => b.CurrentHp > 0).ToList();
      var length = list.Count;
      if (length <= 0)
      {
        return;
      }

      list[0].CurrentHp = list[0].CurrentHp - damage;
      if (length <= 1)
      {
        return;
      }
      var newList = list.Where((b, i) => i > 0).RandomEnumerable().ToList();
      if (targetChained)
      {
        newList.Add(list[0]);
      }
      var damageRemain = damage * damageReduction / 100;
      ChainDamage(chainRemain - 1, damageRemain, damageReduction, targetChained, newList);
      list = null;
      newList = null;
    }
  }
}
