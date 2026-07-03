using Microsoft.AspNetCore.Mvc;
using VulnerableApp.Data;
using System.Linq;

namespace VulnerableApp.Controllers
{
    [ApiController]
    [Route("api")]
    public class ApiController : ControllerBase
    {
        private readonly AppDbContext _db;
        private readonly ILogger<ApiController> _logger;

        public ApiController(AppDbContext db, ILogger<ApiController> logger)
        {
            _db = db;
            _logger = logger;
        }

        [HttpGet("user/{id}")]
        public IActionResult GetUser(int id)
        {
            var currentUserId = HttpContext.Session.GetInt32("UserId");

            _logger.LogInformation("API GetUser ID:{Id}", id);

            if (!currentUserId.HasValue)
            {
                _logger.LogWarning("API acceso sin sesión");
                return Unauthorized();
            }

            if (id != currentUserId.Value)
            {
                _logger.LogWarning("Acceso denegado API User:{User} Intento:{Id}",
                    currentUserId, id);

                return StatusCode(403, "Acceso denegado");
            }

            var user = _db.Users.Find(id);

            if (user == null)
            {
                _logger.LogError("Usuario no encontrado ID:{Id}", id);
                return NotFound();
            }

            return Ok(new { user.Id, user.Username, user.Email });
        }

        [HttpGet("users")]
        public IActionResult GetAllUsers()
        {
            _logger.LogInformation("API GetAllUsers ejecutado");

            var users = _db.Users.Select(u => new
            {
                u.Id,
                u.Username,
                u.Email
            }).ToList();

            return Ok(users);
        }
    }
}