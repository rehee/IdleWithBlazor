using IdleWithBlazor.Common.Interfaces.Actors;
using IdleWithBlazor.Common.Interfaces.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdleWithBlazor.Common.Helpers
{
  public static class TemplateHelper
  {
    public static ITemplateService? templateService { get; private set; }
    public static IItemService itemService { get; private set; }
    public static void AddSingle<T>(T service)
    {
      if (service is ITemplateService t)
      {
        templateService = t;
      }
      else if (service is IItemService i)
      {
        itemService = i;
      }
    }
    private static void InsertTemplate(ITemplate template)
    {
      Task.WaitAll(templateService.AddTemplate(template));
    }
    public static async Task<IGameItem?> GetRamdonDrop(IGameMap map, IDroppable dropper)
    {
      if (templateService == null)
      {
        return null;
      }
      var template = templateService.GetRandomTemplate(map, dropper);
      var item = await itemService.GenerateItemAsync(template);
      template = null;
      if (item is IEquipment equip)
      {
        await itemService.GenerateRandomProperty(equip);
      }

      return item;

    }
  }
}
