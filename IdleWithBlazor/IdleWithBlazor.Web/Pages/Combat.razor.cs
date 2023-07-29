using IdleWithBlazor.Web.Components;

namespace IdleWithBlazor.Web.Pages
{
  public class CombatPage : PageBase
  {
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
      await base.OnAfterRenderAsync(firstRender);
      if (firstRender)
      {
        Room.ValueChange += Room_ValueChange;
      }
    }

    private void Room_ValueChange(object? sender, Common.Events.ContextScopeEventArgs<Model.Actors.GameRoom> e)
    {
      Console.WriteLine("state updated");
      StateHasChanged();
    }

    public override void Dispose()
    {
      Room.ValueChange -= Room_ValueChange;
      base.Dispose();
    }
  }
}
