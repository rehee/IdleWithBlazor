using IdleWithBlazor.Common.DTOs;
using IdleWithBlazor.Common.Enums;
using IdleWithBlazor.Common.Interfaces.Items;
using IdleWithBlazor.Model.GameItems.Templates.Equipments;
using IdleWithBlazor.Server.Services.Items.TemplateServices;
using IdleWithBlazor.Server.Services.Items.ItemServices;

namespace IdleWithBlazor.Test.Items
{
  public class ItemServiceTest
  {
    [SetUp]
    public async Task Setup()
    {
      services = new TemplateService();
      foreach (var bp in TestQueue)
      {
        await services.AddTemplate(bp);
      }
      itemService = new ItemService(services);
    }
    protected ITemplateService services { get; set; }
    protected IItemService itemService { get; set; }
    protected static ITemplate[] TestQueue = new ITemplate[]
    {
      new EquipmentTemplate(EnumEquipment.Body,"body_Aromor"),
      new EquipmentTemplate(EnumEquipment.OneHand,"On_Hand_Sword"),
      new EquipmentTemplate(EnumEquipment.Waist,"waist_Aromor"),
      new EquipmentTemplate(EnumEquipment.Foot,"foot_Aromor"),
    };
    [TestCase("1", EnumItemQuality.Set, 1, true)]
    [TestCase("On_Hand_Sword", EnumItemQuality.Set, 1, false)]
    public async Task ItemGenerateTest(string name, EnumItemQuality quality, int itemLevel, bool isNull)
    {
      var dto = new ItemPrepareDTO
      {
        Template = await services.GetTemplateByName(name),
        Quality = quality,
        ItemLevel = itemLevel,
      };
      var actual = await itemService.GenerateItemAsync(dto);
      if (isNull)
      {
        Assert.IsNull(actual);
        return;
      }
      var Template = await services.GetTemplateByName(name);

      Assert.That(actual.Name, Is.EqualTo(Template.Name));
      Assert.That(actual.ItemType, Is.EqualTo(Template.ItemType));
      if (actual is IEquipment equipment)
      {
        Assert.That(equipment.ItemQuality, Is.EqualTo(quality));
        Assert.That(equipment.ItemLevel, Is.EqualTo(itemLevel));
      }

    }
  }
}
