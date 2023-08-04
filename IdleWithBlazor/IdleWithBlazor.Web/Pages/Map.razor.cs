using IdleWithBlazor.Common.Enums;
using IdleWithBlazor.Web.Components;

namespace IdleWithBlazor.Web.Pages
{
  public class MapPage : PageBase
  {
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
      await base.OnAfterRenderAsync(firstRender);
      if (firstRender)
      {
        this.MapDetail.ValueChange += MapDetail_ValueChange;
        await connection.SetPage(EnumUserPage.Map);
      }
    }

    public override void Dispose()
    {
      base.Dispose();
      this.MapDetail.ValueChange -= MapDetail_ValueChange;
    }
    private void MapDetail_ValueChange(object? sender, Common.Events.ContextScopeEventArgs<Common.DTOs.Actors.GameMapDetailDTO> e)
    {
      StateHasChanged();
    }
  }
}
