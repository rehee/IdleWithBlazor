using IdleWithBlazor.Web.Components;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;

namespace IdleWithBlazor.Web.Pages
{
  public class IndexPage : PageBase
  {
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
      await base.OnAfterRenderAsync(firstRender);
      if (firstRender)
      {
        SkillBook.ValueChange += SkillBook_ValueChange;
      }
    }
    public async Task SelectSkill(Guid skillId, int slot)
    {
      await connection.SelectSkill(skillId, slot);
    }
    public override async ValueTask DisposeAsync()
    {
      await base.DisposeAsync();
      SkillBook.ValueChange -= SkillBook_ValueChange;
    }
    private void SkillBook_ValueChange(object? sender, Common.Events.ContextScopeEventArgs<Common.DTOs.GameActions.Skills.SkillBookDTO> e)
    {

      StateHasChanged();
    }
  }
}
