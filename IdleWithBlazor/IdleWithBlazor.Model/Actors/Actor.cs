using IdleWithBlazor.Common.Interfaces.Actors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace IdleWithBlazor.Model.Actors
{
  public abstract class Actor : IActor
  {
    public abstract Type TypeDiscriminator { get; }
    public Guid Id { get; set; }

    public virtual IEnumerable<IActor> Children { get; set; }
    [JsonIgnore]
    public IActor? Parent { get; protected set; }
    public virtual void SetParent(IActor? actor)
    {
      Parent = actor;
    }

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
    public ValueTask DisposeAsync()
    {

      if (IsDisposed)
      {
        return ValueTask.CompletedTask; ;
      }
      IsDisposed = true;
      GC.SuppressFinalize(this);
      return ValueTask.CompletedTask;
    }

    public virtual Task OnInitialization()
    {
      return Task.CompletedTask;
    }

    public virtual async Task<bool> OnTick()
    {
      if (Children?.Any() == true)
      {
        var results = await Task.WhenAll(Children.Select(b => b.OnTick()).ToArray());
        return results?.All(b => b == true) ?? false;
      }
      return true;
    }
  }
}
