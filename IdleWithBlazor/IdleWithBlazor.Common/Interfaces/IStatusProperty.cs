using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdleWithBlazor.Common.Interfaces
{
  public interface IStatusProperty
  {
    int? Primary { get; set; }
    int? Endurance { get; set; }
    int? Reflection { get; set; }
    int? Will { get; set; }
    int? AllStatus { get; set; }
  }
}
