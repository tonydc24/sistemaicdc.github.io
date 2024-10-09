using Microsoft.AspNetCore.Mvc;
using Iglesia_Página_Web.Models;

namespace Iglesia_Página_Web.Controllers
{
    public class SolicitudesController : Controller
    {
        public IActionResult SolicitudesInicio()
        {
            return View();
        }
    }
}
