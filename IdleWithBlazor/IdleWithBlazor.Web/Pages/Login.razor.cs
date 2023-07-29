using IdleWithBlazor.Common.DTOs;
using IdleWithBlazor.Web.Services;
using Microsoft.AspNetCore.Components;

namespace IdleWithBlazor.Web.Pages
{
  public partial class Login
  {
    [Inject]
    protected NavigationManager nav { get; set; }

    [Inject]
    protected IAuthService auth { get; set; }

    public LoginDTO Dto { get; set; }

    protected override async Task OnInitializedAsync()
    {
      await base.OnInitializedAsync();
      Dto = new LoginDTO()
      {

      };
    }

    public async Task LoginAsync()
    {
      var result = await auth.LoginAsync(Dto);
      if (result)
      {
        nav.NavigateTo("/");
        return;
      }
    }
  }

}
