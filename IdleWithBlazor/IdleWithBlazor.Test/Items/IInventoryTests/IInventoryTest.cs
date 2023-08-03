using IdleWithBlazor.Common.Enums;
using IdleWithBlazor.Common.Helpers;
using IdleWithBlazor.Common.Interfaces.Items;
using IdleWithBlazor.Model.GameItems.Items.Equipments;
using IdleWithBlazor.Model.GameItems.Templates.Equipments;
using NUnit.Framework.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdleWithBlazor.Test.Items.IInventoryTests
{
  public class IInventoryTest : TestBase
  {
    [SetUp]
    public override async Task Setup()
    {
      await base.Setup();
      var itemList = new List<IGameItem>();
      var Template = new EquipmentTemplate(EnumEquipment.OffHand, "副手1");
      for (var i = 0; i < 10; i++)
      {
        itemList.Add(await Template.GenerateGameItemAsync(EnumItemQuality.Unique, 1));
      }
      items = itemList.ToArray();
    }
    private IGameItem[] items;
    [TestCase(1, null, 0, true)]
    [TestCase(2, 0, 0, false)]
    [TestCase(3, 0, 1, true)]
    [TestCase(4, 1, 2, true)]
    public async Task IInventory_PickTest(int index, int? baseIndex, int? itemIndex, bool expectPicked)
    {
      IGameItem? item1 = baseIndex.HasValue ? items[baseIndex.Value] : null;
      IGameItem? item2 = itemIndex.HasValue ? items[itemIndex.Value] : null;
      var count = 0;
      var inventory = ActorHelper.New<IInventory>();
      var isAdded = await inventory.PickItemAsync(item1);
      if (isAdded)
      {
        count++;
      }
      Assert.That(inventory.Items().Count, Is.EqualTo(count));
      var result = await inventory.PickItemAsync(item2);

      Assert.That(result, Is.EqualTo(expectPicked));
      if (result)
      {
        count++;
      }
      Assert.That(inventory.Items().Count, Is.EqualTo(count));
    }
    [TestCase(1, 10, 1, true)]
    [TestCase(2, 0, 1, false)]
    [TestCase(3, 5, 5, false)]
    [TestCase(4, 5, 4, true)]
    public async Task IInventory_takeOutItem_Test(int index, int? insertItemIndex, int itemIndex, bool expectPicked)
    {
      var inventory = ActorHelper.New<IInventory>();
      var count = 0;
      for (var i = 0; i < insertItemIndex; i++)
      {
        if (i >= items.Length)
        {
          break;
        }
        await inventory.PickItemAsync(items[i]);
        count++;
      }
      Assert.That(count, Is.EqualTo(inventory.Items().Count()));
      var item = itemIndex >= items.Length ? null : items[itemIndex];
      if (item == null)
      {
        return;
      }
      var pick = await inventory.TakeOutItemAsync(item.Id);
      Assert.IsTrue((item.Id == pick?.Id) == expectPicked);
      Assert.That(count, Is.EqualTo(inventory.Items().Count()));
    }
    [TestCase(1, 10, 1, true)]
    [TestCase(2, 0, 1, false)]
    [TestCase(3, 5, 5, false)]
    [TestCase(4, 5, 4, true)]
    public async Task IInventory_distoryItem_Test(int index, int? insertItemIndex, int itemIndex, bool expectPicked)
    {
      var inventory = ActorHelper.New<IInventory>();
      var count = 0;
      for (var i = 0; i < insertItemIndex; i++)
      {
        if (i >= items.Length)
        {
          break;
        }
        await inventory.PickItemAsync(items[i]);
        count++;
      }
      Assert.That(count, Is.EqualTo(inventory.Items().Count()));
      var item = itemIndex >= items.Length ? null : items[itemIndex];
      if (item == null)
      {
        return;
      }
      var distory = await inventory.DestoryItemAsync(item.Id);
      Assert.IsTrue(distory == expectPicked);
      if (distory)
      {
        count--;
      }
      Assert.That(count, Is.EqualTo(inventory.Items().Count()));
    }
  }
}
