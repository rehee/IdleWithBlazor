using IdleWithBlazor.Common.Helpers;
using IdleWithBlazor.Common.Jsons.Converters;
using IdleWithBlazor.Model.Actions;
using IdleWithBlazor.Model.Actors;
using IdleWithBlazor.Server.Hubs;
using IdleWithBlazor.Server.Services;
using IdleWithBlazor.Server.Tasks;
using Microsoft.AspNetCore.ResponseCompression;


var builder = WebApplication.CreateBuilder(args);
builder.Services.AddCors(options =>
{
  options.AddDefaultPolicy(builder =>
  {
    builder.AllowAnyOrigin()
           .AllowAnyMethod()
           .AllowAnyHeader();
  });
});
// Add services to the container.
GameService.Room = new GameRoom();
var map = new GameMap();
GameService.Room.Map = map;
var player = new Player();
map.Players = new List<Player> { player };
var mob = new Monster();
map.Monsters = new List<Monster> { mob };
var attack = new Attack();
player.SetAction(attack);
//attack.SetActor(player);
attack.Init(1);
player.SetTarget(mob);

builder.Services.AddSingleton<IGameService, GameService>();

builder.Services.AddHostedService<GameTask>();

builder.Services.AddControllersWithViews().AddJsonOptions(o =>
{
  o.JsonSerializerOptions.SetDefaultOption();
});
builder.Services.AddResponseCompression(opts =>
{
  opts.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(
    new[] { "application/octet-stream" }
    );
});
builder.Services.AddSignalR().AddJsonProtocol(o =>
{
  o.PayloadSerializerOptions.SetDefaultOption();
});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
  app.UseExceptionHandler("/Home/Error");
  // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
  app.UseHsts();
}
app.UseResponseCompression();
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseCors();
app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapHub<MyHub>("/myhub");
app.Run();
