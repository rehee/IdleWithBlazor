using IdleWithBlazor.Common.DTOs.Actors;
using IdleWithBlazor.Common.DTOs.Inventories;
using IdleWithBlazor.Common.Services;
using IdleWithBlazor.Web.Services;
using Microsoft.AspNetCore.Components;

namespace IdleWithBlazor.Web.Components
{
  public abstract class PageBase : ComponentBase, IAsyncDisposable, IDisposable
  {
    [Inject]
    protected IAuthService? auth { get; set; }
    [Inject]
    protected NavigationManager nav { get; set; }
    [Inject]
    protected IConnection connection { get; set; }
    [Inject]
    public IScopedContext<GameRoomDTO> Room { get; protected set; }
    [Inject]
    public IScopedContext<InventoryDTO> Inventory { get; protected set; }

    protected override async Task OnInitializedAsync()
    {
      await base.OnInitializedAsync();
      var isAuth = await auth?.IsAuthenticatedAsync();
      if (isAuth != true)
      {
        nav.NavigateTo("/Login");
        return;
      }
      await connection.ConnectionAsync();
    }

    public virtual ValueTask DisposeAsync()
    {
      Dispose();
      return ValueTask.CompletedTask;
    }
    bool IsDisposed;
    public virtual void Dispose()
    {
      if (IsDisposed)
      {
        return;
      }
      IsDisposed = true;
      
    }

  }
}
