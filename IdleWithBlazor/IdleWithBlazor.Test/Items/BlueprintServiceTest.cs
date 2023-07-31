using IdleWithBlazor.Common.Enums;
using IdleWithBlazor.Common.Interfaces.Items;
using IdleWithBlazor.Model.GameItems.Blurprints.Equipments;
using IdleWithBlazor.Server.Services.Items.BlueprintServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdleWithBlazor.Test.Items
{
  public class BlueprintServiceTest
  {
    protected static IBluePrint[] TestQueue = new IBluePrint[]
    {
      new EquipmentBlueprint(EnumEquipment.Body,"body_Aromor"),
      new EquipmentBlueprint(EnumEquipment.OneHand,"On_Hand_Sword"),
      new EquipmentBlueprint(EnumEquipment.Waist,"waist_Aromor"),
      new EquipmentBlueprint(EnumEquipment.Foot,"foot_Aromor"),
    };
    [TestCase("1", false)]
    [TestCase("body_Aromor", true)]
    [TestCase("body_Aromor2", false)]
    [TestCase("body_Aromor3", false)]
    public async Task BlueprintService_Add_Test(string name, bool isSuccess)
    {
      var bp = TestQueue.FirstOrDefault(x => x.Name == name);
      var bluePrintService = new BlueprintService();
      var actual = await bluePrintService.AddBluePrint(bp);
      Assert.That(actual, Is.EqualTo(isSuccess));
    }
    [TestCase("1")]
    [TestCase("body_Aromor")]
    [TestCase("body_Aromor2")]
    [TestCase("body_Aromor3")]
    public async Task BlueprintService_Get_Test(string name)
    {
      var bluePrintService = new BlueprintService();
      foreach (var bp in TestQueue)
      {
        await bluePrintService.AddBluePrint(bp);
      }
      var actual = await bluePrintService.GetBluePrintByName(name);
      Assert.That(actual, Is.EqualTo(TestQueue.FirstOrDefault(x => x.Name == name)));
    }
    [TestCase("1")]
    [TestCase("body_Aromor")]
    [TestCase("body_Aromor2")]
    [TestCase("body_Aromor3")]
    public async Task BlueprintService_Remove_Test(string name)
    {
      var bluePrintService = new BlueprintService();
      foreach (var bp in TestQueue)
      {
        await bluePrintService.AddBluePrint(bp);
      }
      await bluePrintService.RemoveluePrint(name);
      var actual = await bluePrintService.GetBluePrintByName(name);
      Assert.That(actual, Is.EqualTo(null));
    }
    [Test]
    public async Task BlueprintService_GetRandomItem_HappyPath()
    {
      var bluePrintService = new BlueprintService();
      foreach (var bp in TestQueue)
      {
        await bluePrintService.AddBluePrint(bp);
      }
      for (var i = 0; i < 1000; i++)
      {
        var dto = bluePrintService.GetRandomBlueprint(null, null);
        Assert.IsTrue(dto != null);
        Assert.IsTrue(dto.ItemLevel > 1 && dto.ItemLevel <= 1000);
      }

    }
  }
}
