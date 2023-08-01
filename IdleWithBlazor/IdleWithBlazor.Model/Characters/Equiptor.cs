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
    public override Type TypeDiscriminator => typeof(Equiptor);
    //[JsonIgnore]
    public ConcurrentDictionary<EnumEquipmentSlot, IEquipment> Equipments { get; set; }

    //private Dictionary<EnumEquipmentSlot, IEquipment> _EquipmentMapper { get; set; }
    //public Dictionary<EnumEquipmentSlot, IEquipment> EquipmentMapper
    //{
    //  get
    //  {
    //    if (_EquipmentMapper == null)
    //    {
    //      _EquipmentMapper = Equipments.ToArray().ToDictionary(b => b.Key, b => b.Value);
    //    }
    //    return _EquipmentMapper;
    //  }
    //  set
    //  {
    //    _EquipmentMapper = value;
    //  }
    //}


    public bool Equip(IEquipment equip, int? offset = null)
    {
      Init();
      if (SlotEquiped(equip.EquipmentType, offset))
      {
        return false;
      }
      switch (equip.EquipmentType)
      {
        case EnumEquipment.Head:
          return Equipments.TryAdd(EnumEquipmentSlot.Head, equip);
        case EnumEquipment.Neck:
          return Equipments.TryAdd(EnumEquipmentSlot.Neck, equip);
        case EnumEquipment.Shoulder:
          return Equipments.TryAdd(EnumEquipmentSlot.Shoulder, equip);
        case EnumEquipment.Body:
          return Equipments.TryAdd(EnumEquipmentSlot.Body, equip);
        case EnumEquipment.Hand:
          return Equipments.TryAdd(EnumEquipmentSlot.Hand, equip);
        case EnumEquipment.Finger:
          if (!offset.HasValue || offset == 0)
          {
            return Equipments.TryAdd(EnumEquipmentSlot.LeftFinger, equip);
          }
          return Equipments.TryAdd(EnumEquipmentSlot.Rightinger, equip);

        case EnumEquipment.Waist:
          return Equipments.TryAdd(EnumEquipmentSlot.Waist, equip);
        case EnumEquipment.Wrist:
          return Equipments.TryAdd(EnumEquipmentSlot.Wrist, equip);
        case EnumEquipment.Leg:
          return Equipments.TryAdd(EnumEquipmentSlot.Leg, equip);
        case EnumEquipment.Foot:
          return Equipments.TryAdd(EnumEquipmentSlot.Foot, equip);
        case EnumEquipment.MainHand:
          return Equipments.TryAdd(EnumEquipmentSlot.MainHand, equip);
        case EnumEquipment.OffHand:
          return Equipments.TryAdd(EnumEquipmentSlot.OffHand, equip);
        case EnumEquipment.OneHand:
          if (!offset.HasValue && offset == 0)
          {
            return Equipments.TryAdd(EnumEquipmentSlot.MainHand, equip);
          }
          return Equipments.TryAdd(EnumEquipmentSlot.OffHand, equip);
        case EnumEquipment.TwoHands:
          return Equipments.TryAdd(EnumEquipmentSlot.MainHand, equip);
      }
      return false;
    }
    private bool SlotEquiped(EnumEquipment type, int? offset = null)
    {
      Init();
      IEquipment equip = null;
      switch (type)
      {
        case EnumEquipment.Head:
          return Equipments.TryGetValue(EnumEquipmentSlot.Head, out equip) && equip != null;
        case EnumEquipment.Neck:
          return Equipments.TryGetValue(EnumEquipmentSlot.Neck, out equip) && equip != null;
        case EnumEquipment.Shoulder:
          return Equipments.TryGetValue(EnumEquipmentSlot.Shoulder, out equip) && equip != null;
        case EnumEquipment.Body:
          return Equipments.TryGetValue(EnumEquipmentSlot.Body, out equip) && equip != null;
        case EnumEquipment.Hand:
          return Equipments.TryGetValue(EnumEquipmentSlot.Hand, out equip) && equip != null;
        case EnumEquipment.Finger:
          if (!offset.HasValue || offset == 0)
          {
            return Equipments.TryGetValue(EnumEquipmentSlot.LeftFinger, out equip) && equip != null;
          }
          return Equipments.TryGetValue(EnumEquipmentSlot.Rightinger, out equip) && equip != null;

        case EnumEquipment.Waist:
          return Equipments.TryGetValue(EnumEquipmentSlot.Waist, out equip) && equip != null;
        case EnumEquipment.Wrist:
          return Equipments.TryGetValue(EnumEquipmentSlot.Wrist, out equip) && equip != null;
        case EnumEquipment.Leg:
          return Equipments.TryGetValue(EnumEquipmentSlot.Leg, out equip) && equip != null;
        case EnumEquipment.Foot:
          return Equipments.TryGetValue(EnumEquipmentSlot.Foot, out equip) && equip != null;
        case EnumEquipment.MainHand:
          return Equipments.TryGetValue(EnumEquipmentSlot.MainHand, out equip) && equip != null;
        case EnumEquipment.OffHand:
          return Equipments.TryGetValue(EnumEquipmentSlot.OffHand, out equip) && equip != null;
        case EnumEquipment.OneHand:
          if (!offset.HasValue && offset == 0)
          {
            return Equipments.TryGetValue(EnumEquipmentSlot.MainHand, out equip) && equip != null;
          }
          return Equipments.TryGetValue(EnumEquipmentSlot.OffHand, out equip) && equip != null;
        case EnumEquipment.TwoHands:
          var main = Equipments.TryGetValue(EnumEquipmentSlot.MainHand, out equip) && equip != null;
          var off = Equipments.TryGetValue(EnumEquipmentSlot.OffHand, out equip) && equip != null;
          return main || off;
      }
      return true;
    }
    public IEnumerable<IEquipment?> UnEquip(params EnumEquipmentSlot[] equips)
    {
      Init();
      return equips.Select(b =>
      {
        var gotValue = Equipments.TryRemove(b, out var item);
        return (gotValue, item);
      })
        .Where(b => b.gotValue == true)
      .Select(b => b.item);
    }

    private void Init()
    {
      lock (this)
      {
        if (Equipments == null)
        {
          Equipments = new ConcurrentDictionary<EnumEquipmentSlot, IEquipment>();
        }
      }

    }
  }
}
