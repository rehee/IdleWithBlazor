using IdleWithBlazor.Common.Enums;
using IdleWithBlazor.Model.GameItems.Templates.Equipments;
using IdleWithBlazor.Model.GameItems.Items.Equipments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdleWithBlazor.Test.Items
{
  public class EquipmentTemplateTest
  {
    [TestCase("item1", EnumEquipment.OffHand, EnumItemQuality.Unique, 1)]
    [TestCase("item2", EnumEquipment.Foot, EnumItemQuality.Unique, 1)]
    [TestCase("item3", EnumEquipment.MainHand, EnumItemQuality.Unique, 1)]
    [TestCase("item4", EnumEquipment.Waist, EnumItemQuality.Magic, 1)]
    [TestCase("item5", EnumEquipment.Neck, EnumItemQuality.Unique, 1)]
    [TestCase("item6", EnumEquipment.Body, EnumItemQuality.Normal, 1)]
    [TestCase("item7", EnumEquipment.Foot, EnumItemQuality.Rare, 1)]
    [TestCase("item8", EnumEquipment.Finger, EnumItemQuality.Unique, 1)]
    public async Task EquipmentTemplateCreateTest(string name, EnumEquipment equipment, EnumItemQuality quality, int itemLevel)
    {
      var Template = new EquipmentTemplate(equipment, name);
      var actual = await Template.GenerateGameItemAsync(quality, itemLevel);
      Equipment equip = null;
      bool isEquip = false;
      if (actual is Equipment eq)
      {
        isEquip = true;
        equip = eq;
      }
      Assert.IsTrue(isEquip);
      Assert.That(equip.EquipmentType, Is.EqualTo(equipment));
      Assert.That(equip.ItemType, Is.EqualTo(EnumItemType.Equipment));
      Assert.That(equip.ItemQuality, Is.EqualTo(quality));
      Assert.That(equip.ItemLevel, Is.EqualTo(itemLevel));
      Assert.That(equip.Name, Is.EqualTo(name));
    }
  }
}
