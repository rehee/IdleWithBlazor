using IdleWithBlazor.Common.DTOs;

namespace IdleWithBlazor.Web.Services
{
  public interface IAuthService
  {
    Task<bool> IsAuthenticatedAsync(CancellationToken ct = default(CancellationToken));
    Task<bool> LoginAsync(LoginDTO dto, CancellationToken ct = default(CancellationToken));
    Task<bool> LogOffAsync(CancellationToken ct = default(CancellationToken));
  }
}
