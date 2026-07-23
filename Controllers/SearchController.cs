using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VulnerableApp.Data;
using VulnerableApp.Models;

namespace VulnerableApp.Controllers
{
    public class SearchController : Controller
    {
        private readonly AppDbContext _db;
        private readonly ILogger<SearchController> _logger;

        public SearchController(AppDbContext db, ILogger<SearchController> logger)
        {
            _db = db;
            _logger = logger;
        }

        public IActionResult Index(string search)
        {
            var start = Stopwatch.StartNew();

            _logger.LogInformation("Inicio Search.Index");

            _logger.LogInformation("Usuario:{User} IP:{IP} Search:{Search}",
                HttpContext.Session.GetString("User"),
                HttpContext.Connection.RemoteIpAddress,
                search);

            if (string.IsNullOrEmpty(search))
            {
                _logger.LogWarning("Warning: Search vacio");
                return View(new List<User>());
            }
            var users = _db.Users
                           .Where(u => u.Username.Contains(search))
                           .ToList();

            start.Stop();

            _logger.LogInformation("Resultados encontrados:{Count}", users.Count);
            _logger.LogInformation("Fin Search.Index Tiempo:{Time} ms", start.ElapsedMilliseconds);

            return View(users);
        }
    }
}