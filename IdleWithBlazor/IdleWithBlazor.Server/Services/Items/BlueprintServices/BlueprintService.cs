using IdleWithBlazor.Common.Consts;
using IdleWithBlazor.Common.DTOs;
using IdleWithBlazor.Common.Enums;
using IdleWithBlazor.Common.Helpers;
using IdleWithBlazor.Common.Interfaces;
using IdleWithBlazor.Common.Interfaces.Items;
using IdleWithBlazor.Model.GameItems.Items.Equipments;
using System.Collections.Concurrent;

namespace IdleWithBlazor.Server.Services.Items.BlueprintServices
{
  public class BlueprintService : IBluePrintService
  {
    private static ConcurrentDictionary<string, IBluePrint> mapper = new ConcurrentDictionary<string, IBluePrint>();
    public Task<bool> AddBluePrint(IBluePrint bluePrint, CancellationToken cancellationToken = default)
    {
      if (bluePrint == null)
      {
        return Task.FromResult(false);
      }
      try
      {
        mapper.AddOrUpdate(bluePrint.Name, bluePrint, (n, p) => bluePrint);
        return Task.FromResult(true);
      }
      catch
      {
        return Task.FromResult(false);
      }
    }

    public Task<IBluePrint?> GetBluePrintByName(string name, CancellationToken cancellationToken = default)
    {
      if (mapper.TryGetValue(name, out var bp))
      {
        return Task.FromResult<IBluePrint?>(bp);
      }
      return Task.FromResult<IBluePrint?>(null);
    }

    public Task<bool> RemoveluePrint(string name, CancellationToken cancellationToken = default)
    {
      return Task.FromResult(mapper.TryRemove(name, out var bp));
    }

    public string[] GetRandomProperties(IEquipment equipment)
    {
      int itemTakes = 0;
      switch (equipment.ItemQuality)
      {
        case EnumItemQuality.Magic:
          itemTakes = 2;
          break;
        case EnumItemQuality.Normal:
          itemTakes = 0;
          break;
        default:
          itemTakes = 4;
          break;
      }
      return ItemHelper.GetRandomProperties(itemTakes);
    }
    public double? GetPropertyValue(string name, IEquipment equipment)
    {
      return ConstItem.AllPropertyNumberGrowth[name].GetValue(equipment.ItemLevel);
    }
    public ItemPrepareDTO GetRandomBlueprint(IGameMap? map, IDroppable? actor)
    {
      var rareCount = ItemHelper.GetRandomIntValue(1000, 1, 100);
      EnumItemQuality quality = EnumItemQuality.Normal;
      if (rareCount > 950)
      {
        quality = EnumItemQuality.Unique;
      }
      else if (rareCount > 850)
      {
        quality = EnumItemQuality.Set;
      }
      else if (rareCount > 700)
      {
        quality = EnumItemQuality.Rare;
      }
      else if (rareCount > 400)
      {
        quality = EnumItemQuality.Magic;
      }
      else
      {
        quality = EnumItemQuality.Normal;
      }
      var blueprint = ItemHelper.GetRandomArray(1, mapper.Values.ToArray());
      return new ItemPrepareDTO
      {
        BluePrint = ItemHelper.GetRandomArray(1, mapper.Values.ToArray()).FirstOrDefault(),
        ItemLevel = ItemHelper.GetRandomIntValue(1000, 1, 100).Value,
        Quality = quality
      };
    }
  }
}
