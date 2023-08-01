﻿using IdleWithBlazor.Common.Enums;
using IdleWithBlazor.Common.Interfaces.Items;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdleWithBlazor.Common.Interfaces.Actors
{
  public interface IEquiptor
  {
    ConcurrentDictionary<EnumEquipmentSlot, IEquipment> Equipments { get; set; }
  }
}