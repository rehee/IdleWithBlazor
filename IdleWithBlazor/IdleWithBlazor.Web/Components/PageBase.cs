using IdleWithBlazor.Web.Services;
using Microsoft.AspNetCore.Components;

namespace IdleWithBlazor.Web.Components
{
  public abstract class PageBase : ComponentBase
  {
    [Inject]
    protected IAuthService? auth { get; set; }
    [Inject]
    protected NavigationManager nav { get; set; }
    protected override async Task OnInitializedAsync()
    {
      await base.OnInitializedAsync();
      if (await auth?.IsAuthenticatedAsync() != true)
      {
        nav.NavigateTo("/Login");
        return;
      }

    }

    public string Url { get; set; }
  }
}
