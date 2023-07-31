using IdleWithBlazor.Common.Enums;
using IdleWithBlazor.Common.Interfaces.Actors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdleWithBlazor.Common.Interfaces.Items
{
  public interface IGameItem : IActor
  {
    int ItemLevel { get; set; }
    EnumItemType ItemType { get; }
  }
}
