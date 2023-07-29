using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdleWithBlazor.Model.Actors
{
  public abstract class Actor : IActor
  {
    public int TickCount { get; set; }
    public Guid Id { get; set; }
    public virtual IEnumerable<IActor> Actors { get; set; }
    protected bool IsDisposed { get; set; }
    public virtual void Dispose()
    {
      if (IsDisposed)
      {
        return;
      }
      IsDisposed = true;
      GC.SuppressFinalize(this);
    }
    public virtual async ValueTask DisposeAsync()
    {
      await Task.CompletedTask;
      if (IsDisposed)
      {
        return;
      }
      IsDisposed = true;
      GC.SuppressFinalize(this);

    }

    public virtual Task OnInitialization()
    {
      return Task.CompletedTask;
    }

    public virtual Task OnTick()
    {
      TickCount++;
      if (Actors?.Any() == true)
      {
        Task.WaitAll(Actors.Select(b => b.OnTick()).ToArray());
      }
      return Task.CompletedTask;
    }
  }
}
