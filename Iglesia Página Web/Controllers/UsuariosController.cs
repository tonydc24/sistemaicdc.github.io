using Microsoft.AspNetCore.Mvc;
using Iglesia_Página_Web.Models;

namespace Iglesia_Página_Web.Controllers
{
    public class UsuariosController : Controller
    {

        private readonly ApplicationDbContext _context;

        public UsuariosController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Usuario()
        {
            var usuarios = _context.Usuarios.ToList();
            return View(usuarios);
        }
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(string correo, string contraseña)
        {
            return RedirectToAction("Index", "Home");
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Editar(int id, Usuario usuarioActualizado)
        {
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult Register(Usuario nuevoUsuario)
        {
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]

        public IActionResult Eliminar(int id)
        {
            return RedirectToAction("Index");
        }
    }
}
