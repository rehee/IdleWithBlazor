﻿using IdleWithBlazor.Common.Enums;
using IdleWithBlazor.Common.Interfaces.Actors;
using IdleWithBlazor.Model.Actors;

namespace IdleWithBlazor.Server.Services
{
  public interface IHubServices : IDisposable, IAsyncDisposable
  {
    Task UserConnected(Guid userId, string connectionId);
    Task UserLeave(string connectionId);
    Task SetUserPage(string connectionId, EnumUserPage page);
    Task Broadcast(IEnumerable<ICharacter> characters, IEnumerable<IGameRoom> games);
    Task<IEnumerable<Guid>> ConnectedUsers();

    Task<bool> EquipOrUnequip(string connectionId, Guid? id, int? offset, EnumEquipmentSlot? slot);
    Task SelectSkill(string ConnectionId, Guid skillId, int slot);

    Task QuitGame(string connectionId);
    Task CreateNewGame(string connectionId);
    Task JoinGame(string connectionId, Guid id);
  }
}
