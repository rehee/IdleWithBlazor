using IdleWithBlazor.Common.Enums;
using IdleWithBlazor.Web.Components;

namespace IdleWithBlazor.Web.Pages
{
  public class MiscellPage : PageBase
  {
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
      await base.OnAfterRenderAsync(firstRender);
      if (firstRender)
      {
        this.GameList.ValueChange += GameList_ValueChange;
        await connection.SetPage(EnumUserPage.Miscell);
      }
    }







    public async Task QuitGame()
    {
      await connection.QuitGame();
    }
    public async Task CreateNewGame()
    {
      await connection.CreateNewGame();
    }
    public async Task JoinGame(Guid id)
    {
      await connection.JoinGame(id);
    }

    private void GameList_ValueChange(object? sender, Common.Events.ContextScopeEventArgs<Common.DTOs.Actors.GameListDTO> e)
    {
      StateHasChanged();
    }
    public override void Dispose()
    {
      base.Dispose();
      this.GameList.ValueChange -= GameList_ValueChange;
    }
  }
}
