using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdleWithBlazor.Model.Actors
{
  public interface IActor : IAsyncDisposable, IDisposable
  {
    Guid Id { get; set; }
    Task OnInitialization();
    Task OnTick();
    IEnumerable<IActor> Actors { get; set; }
  }
}
