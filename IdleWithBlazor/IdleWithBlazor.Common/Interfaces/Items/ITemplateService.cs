using IdleWithBlazor.Common.DTOs;
using IdleWithBlazor.Common.Interfaces.Actors;

namespace IdleWithBlazor.Common.Interfaces.Items
{
    public interface ITemplateService
  {
    Task<bool> AddTemplate(ITemplate template, CancellationToken cancellationToken = default);
    Task<ITemplate?> GetTemplateByName(string name, CancellationToken cancellationToken = default);
    Task<bool> RemoveluePrint(string name, CancellationToken cancellationToken = default);
    string[] GetRandomProperties(IEquipment equipment);
    double? GetPropertyValue(string name, IEquipment equipment);
    ItemPrepareDTO GetRandomTemplate(IGameMap map, IDroppable actor);
  }
}
