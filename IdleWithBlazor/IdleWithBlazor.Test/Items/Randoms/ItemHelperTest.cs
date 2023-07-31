using IdleWithBlazor.Common.Helpers;

namespace IdleWithBlazor.Server.Services.Items.NumberRandoms
{
  public class ItemHelperTest
  {
    [TestCase(100, 70, 100)]
    [TestCase(60, 42, 60)]
    public void Random_Int_Test(double defaultValue, int min, int max)
    {
      for (var i = 0; i < 100; i++)
      {
        var actual = ItemHelper.GetRandomIntValue(defaultValue);
        Assert.IsTrue(actual >= min && actual <= max);
      }
    }
    [TestCase(100, 70, 100)]
    [TestCase(60, 42, 60)]
    public void Random_Decimal_Test(double defaultValue, int min, int max)
    {
      for (var i = 0; i < 100; i++)
      {
        var actual = ItemHelper.GetRandomDecimalValue(defaultValue);
        Assert.IsTrue(actual >= min && actual <= max);
      }
    }
    [TestCase(1)]
    [TestCase(2)]
    [TestCase(4)]
    public void ItemHelper_Random_Property(int number)
    {
      var result = ItemHelper.GetRandomProperties(number);
      Assert.That(result.Length, Is.EqualTo(number));

    }
    [TestCaseSource(nameof(ItemHelper_SetRandomlValue_Test_data))]
    public void ItemHelper_SetRandomlValue_Test(string propertyName, double? value, TestSetRandomlValue expect)
    {
      var test = new TestSetRandomlValue();
      ItemHelper.SetRandomlValue(value, typeof(TestSetRandomlValue).GetProperty(propertyName), test, 100, 100);
      Assert.That(test.NullIntValue, Is.EqualTo(expect.NullIntValue));
      Assert.That(test.IntValue, Is.EqualTo(expect.IntValue));

      Assert.IsTrue(Math.Abs(test.DoubleValue - expect.DoubleValue) <= 0.001m);
      Assert.IsTrue(test.NullDoubleValue.HasValue == expect.NullDoubleValue.HasValue);
      if (expect.NullDoubleValue.HasValue)
      {
        Assert.IsTrue(Math.Abs(test.NullDoubleValue.Value - expect.NullDoubleValue.Value) <= 0.001m);
      }

    }
    private static IEnumerable<TestCaseData> ItemHelper_SetRandomlValue_Test_data = new TestCaseData[]
    {
      new TestCaseData(
        nameof(TestSetRandomlValue.NullDoubleValue),
        1.0,
        new TestSetRandomlValue
        {
          NullIntValue=null,
          IntValue=0,
          NullDoubleValue=1m,
          DoubleValue=0m,
        }),
      new TestCaseData(
        nameof(TestSetRandomlValue.NullDoubleValue),
        1.1,
        new TestSetRandomlValue
        {
          NullIntValue=null,
          IntValue=0,
          NullDoubleValue=1.1m,
          DoubleValue=0m,
        }),
      new TestCaseData(
        nameof(TestSetRandomlValue.NullIntValue),
        1.0,
        new TestSetRandomlValue
        {
          NullIntValue=1,
          IntValue=0,
          NullDoubleValue=null,
          DoubleValue=0m,
        }),
      new TestCaseData(
        nameof(TestSetRandomlValue.NullIntValue),
        2.1,
        new TestSetRandomlValue
        {
          NullIntValue=2,
          IntValue=0,
          NullDoubleValue=null,
          DoubleValue=0m,
        })
    };
  }

  public class TestSetRandomlValue
  {
    public int? NullIntValue { get; set; }
    public int IntValue { get; set; }
    public decimal? NullDoubleValue { get; set; }
    public decimal DoubleValue { get; set; }
  }
}
