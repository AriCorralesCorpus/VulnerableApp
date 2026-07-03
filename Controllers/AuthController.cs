using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VulnerableApp.Data;

namespace VulnerableApp.Controllers
{
    public class AuthController : Controller
    {
        private readonly AppDbContext _db;
        private readonly ILogger<AuthController> _logger;

        public AuthController(AppDbContext db, ILogger<AuthController> logger)
        {
            _db = db;
            _logger = logger;
        }

        public IActionResult Login()
        {
            _logger.LogInformation("Inicio Login GET");
            return View();
        }

        [HttpPost]
        public IActionResult Login(string username, string password)
        {
            var start = Stopwatch.StartNew();

            _logger.LogInformation("Intento Login Usuario:{User} IP:{IP}",
                username,
                HttpContext.Connection.RemoteIpAddress);

            var user = _db.Users.FirstOrDefault(u => u.Username == username);

            if (user == null || !BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
            {
                _logger.LogWarning("Login fallido Usuario:{User}", username);

                ViewBag.Error = "Credenciales inválidas";

                start.Stop();
                return View();
            }

            HttpContext.Session.SetString("User", user.Username);
            HttpContext.Session.SetInt32("UserId", user.Id);

            _logger.LogInformation("Login exitoso Usuario:{User}", username);

            start.Stop();

            _logger.LogInformation("Tiempo login:{Time} ms", start.ElapsedMilliseconds);

            return RedirectToAction("Dashboard");
        }

        public IActionResult Dashboard()
        {
            var userId = HttpContext.Session.GetInt32("UserId");

            if (!userId.HasValue)
            {
                _logger.LogWarning("Acceso denegado Dashboard sin sesión");
                return RedirectToAction("Login");
            }

            var user = _db.Users.Find(userId.Value);

            _logger.LogInformation("Acceso Dashboard Usuario:{User}", user?.Username);

            return View(user);
        }

        public IActionResult Logout()
        {
            _logger.LogInformation("Logout Usuario:{User}",
                HttpContext.Session.GetString("User"));

            HttpContext.Session.Clear();

            return RedirectToAction("Index", "Home");
        }
    }
}