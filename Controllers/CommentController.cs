using Microsoft.AspNetCore.Mvc;

namespace VulnerableApp.Controllers
{
    public class CommentController : Controller
    {
        private static List<string> _comments = new();

        private readonly ILogger<CommentController> _logger;

        public CommentController(ILogger<CommentController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            _logger.LogInformation("Inicio Comment.Index");
            _logger.LogInformation("Total comentarios:{Count}", _comments.Count);

            return View(_comments);
        }

        [HttpPost]
        public IActionResult AddComment(string comment)
        {
            _logger.LogInformation("Intento agregar comentario:{Comment}", comment);

            if (string.IsNullOrEmpty(comment))
            {
                _logger.LogWarning("Comentario vacío recibido");
                return RedirectToAction("Index");
            }

            _comments.Add(comment);

            _logger.LogInformation("Comentario agregado correctamente");

            return RedirectToAction("Index");
        }
    }
}