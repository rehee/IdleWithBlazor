using IdleWithBlazor.Common.DTOs;
using IdleWithBlazor.Common.Interfaces.Actors;

namespace IdleWithBlazor.Common.Interfaces.Items
{
    public interface IBluePrintService
  {
    Task<bool> AddBluePrint(IBluePrint bluePrint, CancellationToken cancellationToken = default);
    Task<IBluePrint?> GetBluePrintByName(string name, CancellationToken cancellationToken = default);
    Task<bool> RemoveluePrint(string name, CancellationToken cancellationToken = default);
    string[] GetRandomProperties(IEquipment equipment);
    double? GetPropertyValue(string name, IEquipment equipment);
    ItemPrepareDTO GetRandomBlueprint(IGameMap map, IDroppable actor);
  }
}
