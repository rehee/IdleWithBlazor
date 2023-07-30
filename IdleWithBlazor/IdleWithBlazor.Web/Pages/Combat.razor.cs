using IdleWithBlazor.Common.Enums;
using IdleWithBlazor.Model.Actors;
using IdleWithBlazor.Web.Components;

namespace IdleWithBlazor.Web.Pages
{
  public class CombatPage : PageBase
  {
    public int Count { get; set; }
    public IEnumerable<Monster> Mobs => Room?.Value?.Map?.Monsters?.Select(b => b) ?? Enumerable.Empty<Monster>();
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
      StateHasChanged();
    }
    public async override ValueTask DisposeAsync()
    {
      Room.ValueChange -= Room_ValueChange;
      await base.DisposeAsync();
    }
  }
}
