using IdleWithBlazor.Common.Interfaces.Actors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdleWithBlazor.Common.Interfaces.Items
{
  public interface IInventory : IActor
  {
    IEnumerable<IGameItem>? Items();
    Task<bool> PickItemAsync(IGameItem? item);
    Task<IGameItem?> TakeOutItemAsync(Guid itemId);
    Task<bool> DestoryItemAsync(Guid itemId);
  }
}
