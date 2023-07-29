using IdleWithBlazor.Common.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdleWithBlazor.Common.Services
{
  public interface IScopedContext<T> : IAsyncDisposable, IDisposable
  {
    T? Value { get; }
    void SetValue(T? value);
    void ValueChanged();
    event EventHandler<ContextScopeEventArgs<T>> ValueChange;
  }
}
