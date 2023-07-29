using Blazored.LocalStorage;
using System.Reflection.Metadata.Ecma335;

namespace IdleWithBlazor.Web.Services
{
  public class StorageService : IStorageService
  {
    private readonly ILocalStorageService localStorage;

    public StorageService(ILocalStorageService localStorage)
    {
      this.localStorage = localStorage;
    }
    public async Task<T> GetAsync<T>(string key, CancellationToken ct = default)
    {
      try
      {
        return await localStorage.GetItemAsync<T>(key, ct);
      }
      catch
      {
        return default(T);
      }
    }

    public async Task<bool> SetAsync<T>(string key, T value, CancellationToken ct = default)
    {
      try
      {
        await localStorage.SetItemAsync<T>(key, value, ct);
        return true;
      }
      catch
      {
        return false;
      }
    }
  }
}
