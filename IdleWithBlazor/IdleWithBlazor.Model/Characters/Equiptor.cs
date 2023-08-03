using IdleWithBlazor.Common.Enums;
using IdleWithBlazor.Common.Interfaces.Actors;
using IdleWithBlazor.Common.Interfaces.Items;
using IdleWithBlazor.Model.Actors;
using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace IdleWithBlazor.Model.Characters
{
  public class Equiptor : Actor, IEquiptor
  {
    private ConcurrentDictionary<EnumEquipmentSlot, IEquipment>? equipments { get; set; }
    public override Type TypeDiscriminator => typeof(Equiptor);
    public override void Init(IActor? parent, params object[] setInfo)
    {
      base.Init(parent, setInfo);
      if (equipments == null)
      {
        equipments = new ConcurrentDictionary<EnumEquipmentSlot, IEquipment>();
      }
    }

    public IEnumerable<(EnumEquipmentSlot slot, IEquipment equipment)> Equipments()
    {
      if (equipments != null)
      {
        foreach (var equip in equipments)
        {
          yield return (equip.Key, equip.Value);
        }
      }
    }

    public bool Equip(IEquipment equip, int? offset = null)
    {
      if (SlotEquiped(equip.EquipmentType, offset))
      {
        return false;
      }
      switch (equip.EquipmentType)
      {
        case EnumEquipment.Head:
          return equipments.TryAdd(EnumEquipmentSlot.Head, equip);
        case EnumEquipment.Neck:
          return equipments.TryAdd(EnumEquipmentSlot.Neck, equip);
        case EnumEquipment.Shoulder:
          return equipments.TryAdd(EnumEquipmentSlot.Shoulder, equip);
        case EnumEquipment.Body:
          return equipments.TryAdd(EnumEquipmentSlot.Body, equip);
        case EnumEquipment.Hand:
          return equipments.TryAdd(EnumEquipmentSlot.Hand, equip);
        case EnumEquipment.Finger:
          if (!offset.HasValue || offset == 0)
          {
            return equipments.TryAdd(EnumEquipmentSlot.LeftFinger, equip);
          }
          return equipments.TryAdd(EnumEquipmentSlot.Rightinger, equip);

        case EnumEquipment.Waist:
          return equipments.TryAdd(EnumEquipmentSlot.Waist, equip);
        case EnumEquipment.Wrist:
          return equipments.TryAdd(EnumEquipmentSlot.Wrist, equip);
        case EnumEquipment.Leg:
          return equipments.TryAdd(EnumEquipmentSlot.Leg, equip);
        case EnumEquipment.Foot:
          return equipments.TryAdd(EnumEquipmentSlot.Foot, equip);
        case EnumEquipment.MainHand:
          return equipments.TryAdd(EnumEquipmentSlot.MainHand, equip);
        case EnumEquipment.OffHand:
          return equipments.TryAdd(EnumEquipmentSlot.OffHand, equip);
        case EnumEquipment.OneHand:
          if (!offset.HasValue || offset == 0)
          {
            return equipments.TryAdd(EnumEquipmentSlot.MainHand, equip);
          }
          return equipments.TryAdd(EnumEquipmentSlot.OffHand, equip);
        case EnumEquipment.TwoHands:
          return equipments.TryAdd(EnumEquipmentSlot.MainHand, equip);
      }
      return false;
    }
    public IEnumerable<IEquipment?> UnEquip(params EnumEquipmentSlot[] equips)
    {
      foreach (var equip in equips)
      {
        if (equipments.TryRemove(equip, out var item))
        {
          yield return item;
        }
      }
    }
    private bool SlotEquiped(EnumEquipment type, int? offset = null)
    {
      IEquipment equip = null;
      switch (type)
      {
        case EnumEquipment.Head:
          return equipments.TryGetValue(EnumEquipmentSlot.Head, out equip) && equip != null;
        case EnumEquipment.Neck:
          return equipments.TryGetValue(EnumEquipmentSlot.Neck, out equip) && equip != null;
        case EnumEquipment.Shoulder:
          return equipments.TryGetValue(EnumEquipmentSlot.Shoulder, out equip) && equip != null;
        case EnumEquipment.Body:
          return equipments.TryGetValue(EnumEquipmentSlot.Body, out equip) && equip != null;
        case EnumEquipment.Hand:
          return equipments.TryGetValue(EnumEquipmentSlot.Hand, out equip) && equip != null;
        case EnumEquipment.Finger:
          if (!offset.HasValue || offset == 0)
          {
            return equipments.TryGetValue(EnumEquipmentSlot.LeftFinger, out equip) && equip != null;
          }
          return equipments.TryGetValue(EnumEquipmentSlot.Rightinger, out equip) && equip != null;

        case EnumEquipment.Waist:
          return equipments.TryGetValue(EnumEquipmentSlot.Waist, out equip) && equip != null;
        case EnumEquipment.Wrist:
          return equipments.TryGetValue(EnumEquipmentSlot.Wrist, out equip) && equip != null;
        case EnumEquipment.Leg:
          return equipments.TryGetValue(EnumEquipmentSlot.Leg, out equip) && equip != null;
        case EnumEquipment.Foot:
          return equipments.TryGetValue(EnumEquipmentSlot.Foot, out equip) && equip != null;
        case EnumEquipment.MainHand:
          return equipments.TryGetValue(EnumEquipmentSlot.MainHand, out equip) && equip != null;
        case EnumEquipment.OffHand:
          if (equipments.TryGetValue(EnumEquipmentSlot.MainHand, out equip) && equip.EquipmentType == EnumEquipment.TwoHands)
          {
            return true;
          }
          return equipments.TryGetValue(EnumEquipmentSlot.OffHand, out equip) && equip != null;
        case EnumEquipment.OneHand:
          if (!offset.HasValue || offset == 0)
          {
            return equipments.TryGetValue(EnumEquipmentSlot.MainHand, out equip) && equip != null;
          }
          if (equipments.TryGetValue(EnumEquipmentSlot.MainHand, out equip) && equip.EquipmentType == EnumEquipment.TwoHands)
          {
            return true;
          }
          return equipments.TryGetValue(EnumEquipmentSlot.OffHand, out equip) && equip != null;
        case EnumEquipment.TwoHands:
          var main = equipments.TryGetValue(EnumEquipmentSlot.MainHand, out equip) && equip != null;
          var off = equipments.TryGetValue(EnumEquipmentSlot.OffHand, out equip) && equip != null;
          return main || off;
      }
      return true;
    }

    public override void Dispose()
    {
      Parent = null;
      equipments.Clear();
      equipments = null;
      base.Dispose();
    }
  }
}
