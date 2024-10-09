using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace Iglesia_Página_Web.Controllers
{
    public class CorreosController : Controller
    {
        [HttpGet]
        public IActionResult CorreosInicio()
        { 
           return View();
        }
    }
}
