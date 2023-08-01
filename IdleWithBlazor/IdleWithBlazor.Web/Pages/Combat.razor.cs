using IdleWithBlazor.Common.Consts;
using IdleWithBlazor.Common.Enums;
using IdleWithBlazor.Common.Interfaces.Actors;
using IdleWithBlazor.Model.Actors;
using IdleWithBlazor.Web.Components;
using System.Text.Json;

namespace IdleWithBlazor.Web.Pages
{
  public class CombatPage : PageBase
  {
    public int Count { get; set; }
    public IEnumerable<IMonster> Mobs => Room?.Value?.Map?.Monsters?.Where(b => b != null).Select(b => b) ?? Enumerable.Empty<IMonster>();
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
      await base.OnAfterRenderAsync(firstRender);
      if (firstRender)
      {
        Room.ValueChange += Room_ValueChange;
        await connection.SetPage(EnumUserPage.Combat);
      }
    }

    private void Room_ValueChange(object? sender, Common.Events.ContextScopeEventArgs<Model.Actors.GameRoom> e)
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
