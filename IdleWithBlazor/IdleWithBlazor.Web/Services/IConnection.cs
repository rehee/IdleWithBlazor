namespace IdleWithBlazor.Web.Services
{
  public interface IConnection
  {
    Task<bool> ConnectionAsync();
    Task<bool> AbortAsync();

    Task Send();
    Task KeepSend();
    Task GetRoom();
  }
}
