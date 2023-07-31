using IdleWithBlazor.Common.DTOs;
using IdleWithBlazor.Common.Enums;
using IdleWithBlazor.Common.Interfaces.Items;
using IdleWithBlazor.Model.GameItems.Blurprints.Equipments;
using IdleWithBlazor.Server.Services.Items.BlueprintServices;
using IdleWithBlazor.Server.Services.Items.ItemServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdleWithBlazor.Test.Items
{
  public class ItemServiceTest
  {
    [SetUp]
    public async Task Setup()
    {
      services = new BlueprintService();
      foreach (var bp in TestQueue)
      {
        await services.AddBluePrint(bp);
      }
      itemService = new ItemService(services);
    }
    protected IBluePrintService services { get; set; }
    protected IItemService itemService { get; set; }
    protected static IBluePrint[] TestQueue = new IBluePrint[]
    {
      new EquipmentBlueprint(EnumEquipment.Body,"body_Aromor"),
      new EquipmentBlueprint(EnumEquipment.OneHand,"On_Hand_Sword"),
      new EquipmentBlueprint(EnumEquipment.Waist,"waist_Aromor"),
      new EquipmentBlueprint(EnumEquipment.Foot,"foot_Aromor"),
    };
    [TestCase("1", EnumItemQuality.Set, 1, true)]
    [TestCase("On_Hand_Sword", EnumItemQuality.Set, 1, false)]
    public async Task ItemGenerateTest(string name, EnumItemQuality quality, int itemLevel, bool isNull)
    {
      var dto = new ItemPrepareDTO
      {
        BluePrint = await services.GetBluePrintByName(name),
        Quality = quality,
        ItemLevel = itemLevel,
      };
      var actual = await itemService.GenerateItemAsync(dto);
      if (isNull)
      {
        Assert.IsNull(actual);
        return;
      }
      var bluePrint = await services.GetBluePrintByName(name);

      Assert.That(actual.Name, Is.EqualTo(bluePrint.Name));
      Assert.That(actual.ItemType, Is.EqualTo(bluePrint.ItemType));
      if (actual is IEquipment equipment)
      {
        Assert.That(equipment.ItemQuality, Is.EqualTo(quality));
        Assert.That(equipment.ItemLevel, Is.EqualTo(itemLevel));
      }

    }
  }
}
