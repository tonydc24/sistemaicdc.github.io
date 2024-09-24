using Microsoft.AspNetCore.Mvc;

namespace SistemaWeb.Controllers
{
    public class UsuariosController : Controller
    {
        public IActionResult Sesion()
        {
            return View();
        }
    }
}
