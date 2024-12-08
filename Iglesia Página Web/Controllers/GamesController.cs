using Microsoft.AspNetCore.Mvc;
using Iglesia_Página_Web.Models;

namespace Iglesia_Página_Web.Controllers
{
    public class GamesController : Controller
    {
        [HttpGet]
        public IActionResult GamesInicio()
        {
            return View();
        }
    }
}
