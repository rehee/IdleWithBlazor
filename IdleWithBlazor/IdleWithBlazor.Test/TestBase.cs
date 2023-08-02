using IdleWithBlazor.Model.Helpers;

namespace IdleWithBlazor.Test
{
  public abstract class TestBase
  {
    [SetUp]
    public virtual Task Setup()
    {
      ModelHelper.InitModel();
      return Task.CompletedTask;
    }
  }
}
