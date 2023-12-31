﻿using IdleWithBlazor.Common.Consts;
using IdleWithBlazor.Server.Tasks;
using Microsoft.Extensions.Options;
using Quartz;

namespace IdleWithBlazor.Server.Helpers
{
  public static class TaskHelper
  {
    public static IServiceCollection AddSchedualJob(this IServiceCollection services)
    {
      return services.AddQuartz().AddQuartzHostedService();
    }
  }
  public class TaskConfigration : IConfigureOptions<QuartzOptions>
  {
    public TaskConfigration(IServiceProvider sp)
    {

    }
    public void Configure(QuartzOptions options)
    {
      var key = JobKey.Create(nameof(GameTask2));
      options.AddJob<GameTask2>(jobbuilder => jobbuilder.WithIdentity(key))
       .AddTrigger(trigger =>
       {
         trigger.ForJob(key)
           .WithSimpleSchedule(schedule => schedule.WithInterval(TimeSpan.FromMilliseconds(ConstSetting.TickTime)).RepeatForever());
       });
    }
  }
}
