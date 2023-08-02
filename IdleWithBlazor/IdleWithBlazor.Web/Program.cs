using Blazored.LocalStorage;
using IdleWithBlazor.Common.DTOs.Actors;
using IdleWithBlazor.Common.Services;
using IdleWithBlazor.Web;
using IdleWithBlazor.Web.Services;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.Services.AddBlazoredLocalStorageAsSingleton();
builder.Services.AddSingleton<IStorageService, StorageService>();
builder.Services.AddSingleton<IAuthService, AuthService>();
builder.Services.AddSingleton<IConnection, ConnectionService>();
builder.Services.AddSingleton<IScopedContext<GameRoomDTO>, ScopedContext<GameRoomDTO>>();

builder.Services.AddScoped<IScoped, Scoped>();
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });



await builder.Build().RunAsync();
