using IdleWithBlazor.Common.Consts;
using IdleWithBlazor.Common.DTOs.Actors;
using IdleWithBlazor.Common.Enums;
using IdleWithBlazor.Web.Components;
using System.Text.Json;

namespace IdleWithBlazor.Web.Pages
{
  public class CombatPage : PageBase
  {
    public int Count { get; set; }
    public IEnumerable<MonsterDTO> Mobs => Room?.Value?.GameMap?.Monsters ?? Enumerable.Empty<MonsterDTO>();
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
      await base.OnAfterRenderAsync(firstRender);
      if (firstRender)
      {
        Room.ValueChange += Room_ValueChange;
        await connection.SetPage(EnumUserPage.Combat);
      }
    }

    private void Room_ValueChange(object? sender, Common.Events.ContextScopeEventArgs<GameRoomDTO> e)
    {
      Count++;
      Console.WriteLine(JsonSerializer.Serialize(Room?.Value, ConstSetting.Options));
      StateHasChanged();
    }
    public async override ValueTask DisposeAsync()
    {
      Room.ValueChange -= Room_ValueChange;
      await base.DisposeAsync();
    }
  }
}
