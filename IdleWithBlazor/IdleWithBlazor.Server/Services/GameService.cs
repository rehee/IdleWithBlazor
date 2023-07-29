using IdleWithBlazor.Model.Actors;

namespace IdleWithBlazor.Server.Services
{
  public class GameService : IGameService
  {
    public static GameRoom Room { get; set; }
    public async Task OnTick()
    {
      await Room.OnTick();
    }
  }
}
