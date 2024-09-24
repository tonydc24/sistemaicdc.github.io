using Microsoft.AspNetCore.Mvc;

namespace SistemaWeb.Controllers
{
    public class JuegosController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
