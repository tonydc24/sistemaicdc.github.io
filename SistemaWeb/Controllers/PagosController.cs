using Microsoft.AspNetCore.Mvc;

namespace SistemaWeb.Controllers
{
    public class PagosController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
