using IdleWithBlazor.Common.DTOs;
using IdleWithBlazor.Common.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdleWithBlazor.Common.Interfaces.Items
{
  public interface IItemService
  {
    Task<IGameItem?> GenerateItemAsync(ItemPrepareDTO dto, CancellationToken cancellationToken = default);
    Task GenerateRandomProperty(IEquipment equipment);
  }
}
