using IdleWithBlazor.Common.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdleWithBlazor.Common.Interfaces.Items
{
  public interface IBluePrint : IName
  {
    EnumItemType ItemType { get; }

    Task<IGameItem> GenerateGameItemAsync(EnumItemQuality quality, int itemLevel = 1, CancellationToken cancellationToken = default);
  }
}
