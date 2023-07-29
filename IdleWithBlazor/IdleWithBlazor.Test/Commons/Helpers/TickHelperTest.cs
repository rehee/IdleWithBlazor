using IdleWithBlazor.Common.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdleWithBlazor.Test.Commons.Helpers
{
  public class TickHelperTest
  {
    [TestCase(1, null, null, 0)]
    [TestCase(2, 1, null, 10)]
    [TestCase(3, 1.5, null, 6)]
    [TestCase(4, 2, null, 5)]
    [TestCase(5, null, 1, 10)]
    [TestCase(6, null, 2, 20)]
    [TestCase(7, null, 3, 30)]
    [TestCase(8, null, 1.5, 15)]
    [TestCase(9, null, 2.2, 22)]
    public void TickHelper_GetColdDownTick(
      int index, decimal? attack, decimal? cd, int cdTicket
      )
    {
      var actual = TickHelper.GetColdDownTick(attack, cd);
      Assert.That(actual, Is.EqualTo(cdTicket));
    }
  }
}
