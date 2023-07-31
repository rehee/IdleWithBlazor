using IdleWithBlazor.Common.Enums;
using IdleWithBlazor.Common.Interfaces.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdleWithBlazor.Model.GameItems.Items.Equipments
{
  public class Equipment : GameItemBase, IEquipment
  {
    public override Type TypeDiscriminator => typeof(Equipment);
    public Equipment()
    {

    }
    public EnumItemQuality ItemQuality { get; set; }

    public override EnumItemType ItemType => EnumItemType.Equipment;
    public EnumEquipment EquipmentType { get; set; }
    public int? Primary { get; set; }
    public int? Endurance { get; set; }
    public int? Reflection { get; set; }
    public int? Will { get; set; }
    public int? AllStatus { get; set; }
    public int? GoldFInd { get; set; }
    public int? ExperienceAfterKill { get; set; }
    public int? ExperiencePercentage { get; set; }

    public Equipment(string name, EnumEquipment equipment, EnumItemQuality quality, int itemLevel = 1)
    {
      ItemLevel = itemLevel;
      ItemQuality = quality;
      EquipmentType = equipment;
      Name = name;
    }
  }
}
