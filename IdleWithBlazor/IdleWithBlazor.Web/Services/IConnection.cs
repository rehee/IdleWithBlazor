namespace IdleWithBlazor.Web.Services
{
  public interface IConnection
  {
    Task<bool> ConnectionAsync();
    Task<bool> AbortAsync();
  }
}
