﻿using IdleWithBlazor.Common.Enums;
using IdleWithBlazor.Common.Interfaces.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdleWithBlazor.Model.GameItems.Blurprints
{
  public abstract class TemplateBase : ITemplate
  {
    public TemplateBase()
    {
    }
    public TemplateBase(EnumItemType itemType, string name)
    {
      ItemType = itemType;
      Name = name;
    }
    public virtual EnumItemType ItemType { get; protected set; }
    public virtual string Name { get; set; }
    public abstract Task<IGameItem> GenerateGameItemAsync(EnumItemQuality quality, int itemLevel = 1, CancellationToken cancellationToken = default);
  }
}
