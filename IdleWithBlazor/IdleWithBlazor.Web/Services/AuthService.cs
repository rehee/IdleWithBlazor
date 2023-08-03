using IdleWithBlazor.Common.Consts;
using IdleWithBlazor.Common.DTOs;

namespace IdleWithBlazor.Web.Services
{
  public class AuthService : IAuthService
  {
    private readonly IStorageService storage;

    public AuthService(IStorageService storage)
    {
      this.storage = storage;
    }

    public async Task<bool> IsAuthenticatedAsync(CancellationToken ct = default)
    {
      var token = await storage.GetAsync<string?>(ConstKey.UserAccessToken, ct);
      if (string.IsNullOrEmpty(token))
      {
        return false;
      }
      return true;
    }

    public async Task<bool> LoginAsync(LoginDTO dto, CancellationToken ct = default)
    {
      if (Guid.TryParse(dto.UserName, out Guid userId))
      {
        await storage.SetAsync<string?>(ConstKey.UserAccessToken, dto.UserName);
        await storage.SetAsync<string?>(ConstKey.UserRefreshToken, "token123");
        return true;
      }
      return false;
    }

    public async Task<bool> LogOffAsync(CancellationToken ct = default)
    {
      await storage.SetAsync<string?>(ConstKey.UserAccessToken, null);
      await storage.SetAsync<string?>(ConstKey.UserRefreshToken, null);
      return true;
    }
  }
}
