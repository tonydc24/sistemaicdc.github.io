using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace Iglesia_Página_Web.Controllers
{
    public class CorreosController : Controller
    {
        [HttpGet]
        public IActionResult Correos()
        { 
           return View();
        }

        [HttpPost]
        public IActionResult SendEmail(string subject, string content)
        {
            return RedirectToAction("Correos");
        }

    }
}
