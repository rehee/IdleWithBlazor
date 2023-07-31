using IdleWithBlazor.Common.Enums;
using IdleWithBlazor.Common.Interfaces.Actors;
using IdleWithBlazor.Common.Interfaces.Items;
using IdleWithBlazor.Model.Actors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdleWithBlazor.Model.GameItems
{
  public abstract class GameItemBase : Actor, IGameItem
  {
    public int ItemLevel { get; set; }
    public abstract EnumItemType ItemType { get; }
  }
}
