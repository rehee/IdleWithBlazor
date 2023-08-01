using IdleWithBlazor.Model.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
