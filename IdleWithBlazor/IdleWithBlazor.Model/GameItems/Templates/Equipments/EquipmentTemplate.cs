using IdleWithBlazor.Common.Enums;
using IdleWithBlazor.Common.Interfaces.Items;
using IdleWithBlazor.Model.GameItems.Blurprints;
using IdleWithBlazor.Model.GameItems.Items.Equipments;

namespace IdleWithBlazor.Model.GameItems.Templates.Equipments
{
  public class EquipmentTemplate : TemplateBase
  {
    public EquipmentTemplate() : base()
    {

    }
    public EquipmentTemplate(EnumEquipment equipment, string name) : base(EnumItemType.Equipment, name)
    {
      EquipmentType = equipment;
      Name = name;
    }

    public EnumEquipment EquipmentType { get; protected set; }

    public override Task<IGameItem> GenerateGameItemAsync(EnumItemQuality quality, int itemLevel = 1, CancellationToken cancellationToken = default)
    {
      return Task.FromResult(new Equipment(Name, EquipmentType, quality, itemLevel) as IGameItem);
    }
  }
}
