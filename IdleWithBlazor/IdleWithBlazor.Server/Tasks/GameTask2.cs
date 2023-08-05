using IdleWithBlazor.Common.Consts;
using IdleWithBlazor.Common.Helpers;
using IdleWithBlazor.Server.Services;
using Quartz;

namespace IdleWithBlazor.Server.Tasks
{
  [DisallowConcurrentExecution]
  public class GameTask2 : IJob
  {
    private readonly IHubServices hubService;
    private readonly IServiceProvider sp;
    private readonly IGameService service;
    private int count;

    public GameTask2(IHubServices hubService, IGameService gs, IServiceProvider sp)
    {
      this.hubService = hubService;
      this.sp = sp;
      service = gs;

    }
    public async Task Execute(IJobExecutionContext context)
    {

      await service.OnTick(sp);
      await hubService.Broadcast(service.GetCharacters(), service.Games().Where(b => b.IsClosed != true));


      //if (DateTime.Now.Second % 10 == 0)
      //{
      //  GC.Collect();
      //}

    }
  }
}
