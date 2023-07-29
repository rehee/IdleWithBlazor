using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdleWithBlazor.Common.Events
{
  public class ContextScopeEventArgs<T> : EventArgs
  {
    public T? Value { get; set; }
  }
}
