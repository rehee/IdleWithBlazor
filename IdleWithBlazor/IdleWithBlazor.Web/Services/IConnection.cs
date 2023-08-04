using IdleWithBlazor.Common.Enums;

namespace IdleWithBlazor.Web.Services
{
  public interface IConnection
  {
    Task<bool> ConnectionAsync();
    Task<bool> AbortAsync();

    Task Send();
    Task KeepSend();
    Task SetPage(EnumUserPage page);
    Task<bool> EquipItem(Guid? id, int? offset);
    Task<bool> UnEquipItem(EnumEquipmentSlot slot);
    Task SelectSkill(Guid skillId, int slot);

    Task QuitGame();
    Task CreateNewGame();
    Task JoinGame(Guid id);
  }
}
