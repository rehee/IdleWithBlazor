using IdleWithBlazor.Common.Interfaces.Items;
using IdleWithBlazor.Model.Actors;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdleWithBlazor.Model.GameItems.Inventories
{
  public class Inventory : Actor, IInventory
  {
    public Inventory()
    {
      inventoryMapper = new ConcurrentDictionary<Guid, IGameItem>();
    }
    public override Type TypeDiscriminator => typeof(Inventory);
    private ConcurrentDictionary<Guid, IGameItem>? inventoryMapper { get; set; }
    public IEnumerable<IGameItem>? InventoryList => inventoryMapper?.Values;

    public Task<bool> DestoryItemAsync(Guid itemId)
    {
      return Task.FromResult(
        inventoryMapper?.TryRemove(itemId, out var item) ?? false
        );
    }

    public Task<IGameItem?> TakeOutItemAsync(Guid itemId)
    {
      if (inventoryMapper?.TryGetValue(itemId, out var item) == true)
      {
        return Task.FromResult(item ?? default(IGameItem?));
      }
      return Task.FromResult(default(IGameItem?));
    }

    public Task<bool> PickItemAsync(IGameItem? item)
    {
      if (item == null)
      {
        return Task.FromResult(false);
      }
      return Task.FromResult(inventoryMapper?.TryAdd(item.Id, item) ?? false);
    }
  }
}
