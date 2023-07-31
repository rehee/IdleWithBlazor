using IdleWithBlazor.Common.Helpers;
using IdleWithBlazor.Common.Interfaces.Items;
using IdleWithBlazor.Common.Jsons.Converters;
using IdleWithBlazor.Model.Actions;
using IdleWithBlazor.Model.Actors;
using IdleWithBlazor.Server.Hubs;
using IdleWithBlazor.Server.Services;
using IdleWithBlazor.Server.Services.Items.BlueprintServices;
using IdleWithBlazor.Server.Services.Items.ItemServices;
using IdleWithBlazor.Server.Tasks;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.AspNetCore.SignalR;

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


builder.Services.AddScoped<IHubServices, HubServices>(sp =>
  new HubServices(sp.GetService<IHubContext<MyHub>>()));
builder.Services.AddSingleton<IBluePrintService, BlueprintService>();
builder.Services.AddSingleton<IItemService, ItemService>();
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
