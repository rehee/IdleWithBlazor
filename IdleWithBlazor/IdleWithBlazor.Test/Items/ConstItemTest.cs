using IdleWithBlazor.Common.Consts;
using IdleWithBlazor.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdleWithBlazor.Test.Items
{
  public class ConstItemTest
  {
    [Test]
    public void ConstItem_Status_Test()
    {
      var properties = ConstItem.StatusProperty;
      var expect = new string[]
      {
        nameof(IStatusProperty.Primary),
        nameof(IStatusProperty.Will),
        nameof(IStatusProperty.Reflection),
        nameof(IStatusProperty.Endurance),
        nameof(IStatusProperty.AllStatus),
      };
      Assert.That(properties, Is.EquivalentTo(expect));
    }
  }
}
