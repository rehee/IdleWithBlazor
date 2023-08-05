using IdleWithBlazor.Web.Components;

namespace IdleWithBlazor.Web.Pages
{
  public class SystemsPage : PageBase
  {
    public async Task LogoutAsync()
    {
      await auth?.LogOffAsync();
      nav.NavigateTo("Login", true);
    }
  }


}
