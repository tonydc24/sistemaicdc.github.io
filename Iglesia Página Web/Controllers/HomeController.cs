using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Iglesia_Página_Web.Data;

using Iglesia_Página_Web.Models;
using System.Diagnostics;

namespace Iglesia_Página_Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            ViewData["Title"] = "Iglesia Casa de Dios Coronado";
            return View();
        }

        [HttpGet]
        public IActionResult Correos()
        {
            return View();
        }

        [HttpGet]

        public IActionResult Games()
        {
            return View();
        }

        [HttpGet]

        public IActionResult Inventario()
        {
            return View();
        }

        public IActionResult Solicitudes()
        {
            return View();
        }
       
        public IActionResult Usuario()
        {
            return View();
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
