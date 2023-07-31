using IdleWithBlazor.Common.Enums.Numbers;
using IdleWithBlazor.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdleWithBlazor.Test.Items
{
  public class NumberGenerateTesy
  {
    [TestCaseSource(nameof(NumberGenerate_Test_data))]
    public void NumberGenerate_Test(int index, NumberGrowth ng, int level, double expect)
    {
      var value = ng.GetValue(level);
      Assert.IsTrue(Math.Abs(expect - value ?? 0d) <= 0.001);
    }

    private static IEnumerable<TestCaseData> NumberGenerate_Test_data = new TestCaseData[]
    {
      new TestCaseData(
        1,
        new NumberGrowth
        {
          Growth= EnumNumberGrowth.Linear,
          BaseValue=1,
          GrowthNumber= 1,
        },
        1
        ,1d
        ),
      new TestCaseData(
        2,
        new NumberGrowth
        {
          Growth= EnumNumberGrowth.Linear,
          BaseValue=1,
          GrowthNumber= 1,
        },
        2
        ,2d
        ),
      new TestCaseData(
        3,
        new NumberGrowth
        {
          Growth= EnumNumberGrowth.Linear,
          BaseValue=1,
          GrowthNumber= 1,
        },
        3
        ,3d
        ),
       new TestCaseData(
        4,
        new NumberGrowth
        {
          Growth= EnumNumberGrowth.Linear,
          BaseValue=0.5,
          GrowthNumber= 1,
        },
        3
        ,2.5d
        ),
       new TestCaseData(
        5,
        new NumberGrowth
        {
          Growth= EnumNumberGrowth.Linear,
          BaseValue=1,
          GrowthNumber= 0.5,
        },
        3
        ,2d
        ),
        new TestCaseData(
        6,
        new NumberGrowth
        {
          Growth= EnumNumberGrowth.Logarithmic,
          BaseValue=10,
          GrowthNumber= 0.1,
        },
        2
        ,11d
        ),
        new TestCaseData(
        7,
        new NumberGrowth
        {
          Growth= EnumNumberGrowth.Logarithmic,
          BaseValue=10,
          GrowthNumber= 0.1,
        },
        1
        ,10d
        ),
         new TestCaseData(
        8,
        new NumberGrowth
        {
          Growth= EnumNumberGrowth.Logarithmic,
          BaseValue=10,
          GrowthNumber= 0.1,
        },
        3
        ,12.1d
        ),
    };

  }
}
