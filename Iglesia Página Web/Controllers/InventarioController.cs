using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Iglesia_Página_Web.Models;

namespace Iglesia_Página_Web.Controllers
{
    public class InventarioController : Controller
    {
        public IActionResult InventarioInicio()
        {
            return View();
        }
    }
}
