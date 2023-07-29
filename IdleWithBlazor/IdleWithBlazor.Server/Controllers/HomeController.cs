﻿using IdleWithBlazor.Model.Actors;
using IdleWithBlazor.Server.Models;
using IdleWithBlazor.Server.Services;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Text.Json;

namespace IdleWithBlazor.Server.Controllers
{
  public class HomeController : Controller
  {
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
      _logger = logger;
    }

    public IActionResult Index()
    {
      var json = JsonSerializer.Serialize(GameService.Room);
      var obj = JsonSerializer.Deserialize<GameRoom>(json);
      var hp = obj.Map.Monsters.FirstOrDefault()?.CurrentMp;
      return Ok(GameService.Room);
    }

    public IActionResult Privacy()
    {
      return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
      return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
  }
}