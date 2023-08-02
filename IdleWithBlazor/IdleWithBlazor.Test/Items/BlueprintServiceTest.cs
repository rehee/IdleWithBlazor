using IdleWithBlazor.Common.Enums;
using IdleWithBlazor.Common.Interfaces.Items;
using IdleWithBlazor.Model.GameItems.Blurprints.Equipments;
using IdleWithBlazor.Server.Services.Items.TemplateServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdleWithBlazor.Test.Items
{
  public class TemplateServiceTest
  {
    protected static ITemplate[] TestQueue = new ITemplate[]
    {
      new EquipmentTemplate(EnumEquipment.Body,"body_Aromor"),
      new EquipmentTemplate(EnumEquipment.OneHand,"On_Hand_Sword"),
      new EquipmentTemplate(EnumEquipment.Waist,"waist_Aromor"),
      new EquipmentTemplate(EnumEquipment.Foot,"foot_Aromor"),
    };
    [TestCase("1", false)]
    [TestCase("body_Aromor", true)]
    [TestCase("body_Aromor2", false)]
    [TestCase("body_Aromor3", false)]
    public async Task TemplateService_Add_Test(string name, bool isSuccess)
    {
      var bp = TestQueue.FirstOrDefault(x => x.Name == name);
      var TemplateService = new TemplateService();
      var actual = await TemplateService.AddTemplate(bp);
      Assert.That(actual, Is.EqualTo(isSuccess));
    }
    [TestCase("1")]
    [TestCase("body_Aromor")]
    [TestCase("body_Aromor2")]
    [TestCase("body_Aromor3")]
    public async Task TemplateService_Get_Test(string name)
    {
      var TemplateService = new TemplateService();
      foreach (var bp in TestQueue)
      {
        await TemplateService.AddTemplate(bp);
      }
      var actual = await TemplateService.GetTemplateByName(name);
      Assert.That(actual, Is.EqualTo(TestQueue.FirstOrDefault(x => x.Name == name)));
    }
    [TestCase("1")]
    [TestCase("body_Aromor")]
    [TestCase("body_Aromor2")]
    [TestCase("body_Aromor3")]
    public async Task TemplateService_Remove_Test(string name)
    {
      var TemplateService = new TemplateService();
      foreach (var bp in TestQueue)
      {
        await TemplateService.AddTemplate(bp);
      }
      await TemplateService.RemoveluePrint(name);
      var actual = await TemplateService.GetTemplateByName(name);
      Assert.That(actual, Is.EqualTo(null));
    }
    [Test]
    public async Task TemplateService_GetRandomItem_HappyPath()
    {
      var TemplateService = new TemplateService();
      foreach (var bp in TestQueue)
      {
        await TemplateService.AddTemplate(bp);
      }
      for (var i = 0; i < 1000; i++)
      {
        var dto = TemplateService.GetRandomTemplate(null, null);
        Assert.IsTrue(dto != null);
        Assert.IsTrue(dto.ItemLevel > 1 && dto.ItemLevel <= 1000);
      }

    }
  }
}
