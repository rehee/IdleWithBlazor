using IdleWithBlazor.Common.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdleWithBlazor.Common.Services
{
  public class ScopedContext<T> : IScopedContext<T>
  {
    public T? Value { get; protected set; }

    List<EventHandler<ContextScopeEventArgs<T>>> delegates = new List<EventHandler<ContextScopeEventArgs<T>>>();
    private event EventHandler<ContextScopeEventArgs<T>> valueChange;
    public virtual event EventHandler<ContextScopeEventArgs<T>> ValueChange
    {
      add
      {
        valueChange += value;
        delegates.Add(value);
      }
      remove
      {
        valueChange -= value;
        delegates.Remove(value);
      }

    }

    public void SetValue(T? value)
    {
      Value = value;
      ValueChanged();
    }

    public void ValueChanged()
    {
      if (valueChange != null)
      {
        valueChange(this, new ContextScopeEventArgs<T>() { Value = Value });
      }
    }
    protected bool _disposed { get; set; }
    public void Dispose()
    {
      if (_disposed)
      {
        return;
      }
      _disposed = true;
      try
      {
        var list = delegates.Select(b => b).ToArray();
        foreach (var d in list)
        {
          ValueChange -= d;
        }
        delegates.Clear();
        list = null;
        GC.SuppressFinalize(this);
      }
      catch { }
    }
    public ValueTask DisposeAsync()
    {
      Dispose();
      return ValueTask.CompletedTask;
    }
  }
}
