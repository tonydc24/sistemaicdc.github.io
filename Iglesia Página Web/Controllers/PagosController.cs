using Microsoft.AspNetCore.Mvc;
using Iglesia_Página_Web.Models;
using System.Collections.Generic;

namespace Iglesia_Página_Web.Controllers
{
    public class PagosController : Controller
    {
        public IActionResult PagosInicio()
        {
            return View();
        }
    }
}
