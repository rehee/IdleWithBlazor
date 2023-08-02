using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdleWithBlazor.Common.Interfaces.Actors
{
  public interface ILevelupable : ILeveled
  {
    int CurrentExp { get; set; }
    int NextLevelExp { get; set; }
    bool EnableLevelUp { get; set; }
  }
  public interface ILeveled
  {
    int Level { get; set; }
  }
}
