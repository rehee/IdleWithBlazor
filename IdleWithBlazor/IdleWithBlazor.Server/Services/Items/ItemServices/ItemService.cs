using IdleWithBlazor.Common.Consts;
using IdleWithBlazor.Common.DTOs;
using IdleWithBlazor.Common.Enums;
using IdleWithBlazor.Common.Helpers;
using IdleWithBlazor.Common.Interfaces.Items;

namespace IdleWithBlazor.Server.Services.Items.ItemServices
{
  public class ItemService : IItemService
  {
    private ITemplateService TemplateService { get; set; }
    public ItemService(ITemplateService TemplateService)
    {
      this.TemplateService = TemplateService;
    }

    public async Task<IGameItem?> GenerateItemAsync(ItemPrepareDTO dto, CancellationToken cancellationToken = default)
    {
      var Template = dto.Template;
      if (Template == null)
      {
        return null;
      }
      return await Template.GenerateGameItemAsync(dto.Quality, dto.ItemLevel, cancellationToken);
    }

    public Task GenerateRandomProperty(IEquipment equipment)
    {
      var properties = TemplateService.GetRandomProperties(equipment);
      var typeProperties = equipment.GetType().GetProperties();
      foreach (var property in properties)
      {
        var value = TemplateService.GetPropertyValue(property, equipment);
        if (value == null)
        {
          continue;
        }
        var info = typeProperties.FirstOrDefault(b => String.Equals(b.Name, property, StringComparison.OrdinalIgnoreCase));
        if (info == null)
        {
          continue;
        }
        ItemHelper.SetRandomlValue(value, info, equipment);
      }
      return Task.CompletedTask;
    }

  }
}
