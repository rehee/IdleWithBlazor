using IdleWithBlazor.Common.Enums;
using IdleWithBlazor.Common.Interfaces.Items;
using IdleWithBlazor.Model.Characters;
using IdleWithBlazor.Model.GameItems.Items.Equipments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdleWithBlazor.Test.Actors
{
  public class EquiptorTest
  {
    [TestCaseSource(nameof(Equiptor_Test_data))]
    public void Equiptor_Test(int index, IEnumerable<IEquipment> preEquip, IEquipment equip, int offset, bool expected)
    {
      var equipter = new Equiptor();
      foreach (var item in preEquip)
      {
        var success = equipter.Equip(item);
        Assert.IsTrue(success);
      }
      var actual = equipter.Equip(equip, offset);
      Assert.That(actual, Is.EqualTo(expected));
    }
    private static IEnumerable<TestCaseData> Equiptor_Test_data = new TestCaseData[]
    {
      new TestCaseData(
        1,
        new IEquipment[]
        {

        },
        new Equipment
        {
          EquipmentType = EnumEquipment.Body,
        },
        0,
        true
        ),
      new TestCaseData(
        2,
        new IEquipment[]
        {
          new Equipment
        {
          EquipmentType = EnumEquipment.Finger,
        },
        },
        new Equipment
        {
          EquipmentType = EnumEquipment.Finger,
        },
        1,
        true
        ),
      new TestCaseData(
        3,
        new IEquipment[]
        {
          new Equipment
        {
          EquipmentType = EnumEquipment.Finger,
        },
        },
        new Equipment
        {
          EquipmentType = EnumEquipment.Finger,
        },
        0,
        false
        ),
    };
    [TestCaseSource(nameof(Un_Equiptor_Test_data))]
    public void Un_Equiptor_Test(int index, IEnumerable<IEquipment> preEquip, EnumEquipmentSlot equip, int offset, bool expected)
    {
      var equipter = new Equiptor();
      foreach (var item in preEquip)
      {
        var success = equipter.Equip(item, offset);
        Assert.IsTrue(success);
      }
      var actual = equipter.UnEquip(equip);
      Assert.That(actual.FirstOrDefault() != null, Is.EqualTo(expected));
    }
    private static IEnumerable<TestCaseData> Un_Equiptor_Test_data = new TestCaseData[]
    {
      new TestCaseData(
        1,
        new IEquipment[]
        {
          new Equipment("1",EnumEquipment.Body, EnumItemQuality.Unique,1),
        },
        EnumEquipmentSlot.Body,
        0,
        true),
      new TestCaseData(
        2,
        new IEquipment[]
        {
          new Equipment("1",EnumEquipment.Body, EnumItemQuality.Unique,1),
        },
        EnumEquipmentSlot.OffHand,
        0,
        false),
      new TestCaseData(
        3,
        new IEquipment[]
        {
          new Equipment("1",EnumEquipment.Body, EnumItemQuality.Unique,1),
        },
        EnumEquipmentSlot.Body,
        1,
        true),
      new TestCaseData(
        4,
        new IEquipment[]
        {
          new Equipment("1",EnumEquipment.Finger, EnumItemQuality.Unique,1),
        },
        EnumEquipmentSlot.Rightinger,
        1,
        true),
      new TestCaseData(
        5,
        new IEquipment[]
        {
          new Equipment("1",EnumEquipment.Finger, EnumItemQuality.Unique,1),
        },
        EnumEquipmentSlot.LeftFinger,
        1,
        false),
      new TestCaseData(
        6,
        new IEquipment[]
        {
          new Equipment("1",EnumEquipment.TwoHands, EnumItemQuality.Unique,1),
        },
        EnumEquipmentSlot.OffHand,
        1,
        false),
      new TestCaseData(
        6,
        new IEquipment[]
        {
          new Equipment("1",EnumEquipment.TwoHands, EnumItemQuality.Unique,1),
        },
        EnumEquipmentSlot.MainHand,
        1,
        true),
    };
  }
}
