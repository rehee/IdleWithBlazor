namespace IdleWithBlazor.Web
{
  public interface IScoped : IDisposable
  {
    Guid Id { get; }
  }

  public class Scoped : IScoped
  {
    public Scoped()
    {
      Id = Guid.NewGuid();
    }
    public Guid Id
    {
      get; private set;
    }

    public void Dispose()
    {
      var a = 1;
    }
  }
}
