namespace IdleWithBlazor.Web.Services
{
  public interface IStorageService
  {
    Task<T> GetAsync<T>(string key, CancellationToken ct = default(CancellationToken));
    Task<bool> SetAsync<T>(string key, T value, CancellationToken ct = default(CancellationToken));
  }
}
