using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TryMultiActor.Actors
{
  public class Actor : IActor
  {
    public IEnumerable<IActor> Children => throw new NotImplementedException();

    public Task OnTick()
    {
      throw new NotImplementedException();
    }
  }
  public class CActor : IActor
  {
    public IEnumerable<IActor> Children => throw new NotImplementedException();

    public Task OnTick()
    {
      throw new NotImplementedException();
    }
  }
  public interface IActor
  {
    public IEnumerable<IActor> Children { get; }
    Task OnTick();
  }
}
