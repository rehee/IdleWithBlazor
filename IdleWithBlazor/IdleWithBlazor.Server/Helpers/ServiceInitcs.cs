using IdleWithBlazor.Common.Enums;
using IdleWithBlazor.Common.Helpers;
using IdleWithBlazor.Common.Interfaces.Items;
using IdleWithBlazor.Model.GameItems.Templates.Equipments;
using IdleWithBlazor.Server.Services.Items.ItemServices;
using IdleWithBlazor.Server.Services.Items.TemplateServices;

namespace IdleWithBlazor.Server.Helpers
{
  public static class ServiceInitcs
  {
    public static ITemplate[] Templates => new ITemplate[]
    {
      new EquipmentTemplate(EnumEquipment.Neck,"小项链"),
      new EquipmentTemplate(EnumEquipment.OneHand,"小匕首"),
      new EquipmentTemplate(EnumEquipment.MainHand,"镰刀"),
      new EquipmentTemplate(EnumEquipment.OffHand,"盾牌"),
      new EquipmentTemplate(EnumEquipment.Body,"盔甲"),
      new EquipmentTemplate(EnumEquipment.Head,"渔夫帽"),
      new EquipmentTemplate(EnumEquipment.Waist,"腰缠"),
      new EquipmentTemplate(EnumEquipment.Finger,"戒指"),
      new EquipmentTemplate(EnumEquipment.Finger,"扳指"),
      new EquipmentTemplate(EnumEquipment.Leg,"护腿"),
      new EquipmentTemplate(EnumEquipment.Shoulder,"护肩"),
      new EquipmentTemplate(EnumEquipment.Wrist,"护腕"),
      new EquipmentTemplate(EnumEquipment.OneHand, "匕首"),
      new EquipmentTemplate(EnumEquipment.TwoHands, "大斧头")
    };

    public static void AddSingleTemplate()
    {
      var templateService = new TemplateService();
      Task.WaitAll(Templates.Select(b => templateService.AddTemplate(b)).ToArray());
      var itemService = new ItemService(templateService);

      TemplateHelper.AddSingle<ITemplateService>(templateService);
      TemplateHelper.AddSingle<IItemService>(itemService);


    }

  }
}
