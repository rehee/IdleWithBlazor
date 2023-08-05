using Blazored.LocalStorage;
using IdleWithBlazor.Common.DTOs.Actors;
using IdleWithBlazor.Common.DTOs.GameActions.Skills;
using IdleWithBlazor.Common.DTOs.Inventories;
using IdleWithBlazor.Common.Helpers;
using IdleWithBlazor.Common.Services;
using IdleWithBlazor.Web;
using IdleWithBlazor.Web.Models;
using IdleWithBlazor.Web.Services;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

var setting = new Setting
{
  ServerHost = builder.Configuration["ServerHost"],
  ReBaseUrl = builder.Configuration["ReBaseUrl"]
};
Console.WriteLine(setting.ServerHost);
builder.Services.AddSingleton<Setting>(sp => setting);
builder.Services.AddBlazoredLocalStorageAsSingleton();
builder.Services.AddSingleton<IStorageService, StorageService>();
builder.Services.AddSingleton<IAuthService, AuthService>();
builder.Services.AddSingleton<IConnection, ConnectionService>();
builder.Services.AddSingleton<IScopedContext<GameRoomDTO>, ScopedContext<GameRoomDTO>>();
builder.Services.AddSingleton<IScopedContext<InventoryDTO>, ScopedContext<InventoryDTO>>();
builder.Services.AddSingleton<IScopedContext<SkillBookDTO>, ScopedContext<SkillBookDTO>>();
builder.Services.AddSingleton<IScopedContext<GameMapDetailDTO>, ScopedContext<GameMapDetailDTO>>();
builder.Services.AddSingleton<IScopedContext<GameListDTO>, ScopedContext<GameListDTO>>();
builder.Services.AddScoped<IScoped, Scoped>();
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });



await builder.Build().RunAsync();
