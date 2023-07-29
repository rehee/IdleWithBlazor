using IdleWithBlazor.Model.Actors;

namespace IdleWithBlazor.Server.Models
{
  public class MyActor : Actor
  {
    public override async Task OnTick()
    {
      Console.WriteLine("ticked");

      await base.OnTick();
    }
  }
}
