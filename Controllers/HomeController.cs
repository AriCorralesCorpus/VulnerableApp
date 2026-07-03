using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using VulnerableApp.Models;
using Serilog;

namespace VulnerableApp.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }
    public IActionResult Index()
    {
        var start = Stopwatch.StartNew();

        _logger.LogInformation("Inicio Home.Index");

        _logger.LogInformation("Usuario: {User} IP: {IP}",
            HttpContext.Session.GetString("User"),
            HttpContext.Connection.RemoteIpAddress);

        var result = View();

        start.Stop();

        _logger.LogInformation("Fin Home.Index Tiempo: {Time} ms", 
            start.ElapsedMilliseconds);

        return result;
    }

    public IActionResult Privacy()
    {
        _logger.LogInformation("Inicio Privacy");
        _logger.LogInformation("Fin Privacy");
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        _logger.LogError("Error en HomeController");
        return View();
    }
}
