using Microsoft.AspNetCore.Mvc;

namespace SistemaWeb.Controllers
{
    public class InventarioController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
