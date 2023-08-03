using IdleWithBlazor.Common.Consts;
using IdleWithBlazor.Common.DTOs;
using IdleWithBlazor.Common.Enums;
using IdleWithBlazor.Common.Helpers;
using IdleWithBlazor.Common.Interfaces.Actors;
using IdleWithBlazor.Common.Interfaces.Items;
using System.Collections.Concurrent;

namespace IdleWithBlazor.Server.Services.Items.TemplateServices
{
  public class TemplateService : ITemplateService
    {
    private static ConcurrentDictionary<string, ITemplate> mapper = new ConcurrentDictionary<string, ITemplate>();
    public Task<bool> AddTemplate(ITemplate template, CancellationToken cancellationToken = default)
    {
      if (template == null)
      {
        return Task.FromResult(false);
      }
      try
      {
        mapper.AddOrUpdate(template.Name, template, (n, p) => template);
        return Task.FromResult(true);
      }
      catch
      {
        return Task.FromResult(false);
      }
    }

    public Task<ITemplate?> GetTemplateByName(string name, CancellationToken cancellationToken = default)
    {
      if (mapper.TryGetValue(name, out var bp))
      {
        return Task.FromResult<ITemplate?>(bp);
      }
      return Task.FromResult<ITemplate?>(null);
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
    public ItemPrepareDTO GetRandomTemplate(IGameMap? map, IDroppable? actor)
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
      var Template = ItemHelper.GetRandomArray(1, mapper.Values.ToArray());
      return new ItemPrepareDTO
      {
        Template = ItemHelper.GetRandomArray(1, mapper.Values.ToArray()).FirstOrDefault(),
        ItemLevel = ItemHelper.GetRandomIntValue(1000, 1, 100).Value,
        Quality = quality
      };
    }
  }
}
