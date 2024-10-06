using Microsoft.AspNetCore.Mvc;
using Iglesia_Página_Web.Models;

namespace Iglesia_Página_Web.Controllers
{
    public class SolicitudesController : Controller
    {
        public IActionResult Solicitudes()
        {
            var solicitudes = new List<Solicitud>
            {
                new Solicitud { SolicitudId = "001", UsuarioId = "U1001", DetalleSolicitud = "Problema de acceso al sistema", Estado = "Pendiente", FechaSolicitud = new DateTime(2024, 8, 15) },
                new Solicitud { SolicitudId = "002", UsuarioId = "U1002", DetalleSolicitud = "Error en el pago", Estado = "Resuelto", FechaSolicitud = new DateTime(2024, 8, 16) },
                new Solicitud { SolicitudId = "003", UsuarioId = "U1003", DetalleSolicitud = "Cambio de dirección", Estado = "En Proceso", FechaSolicitud = new DateTime(2024, 8, 17) }
            };

            return View(solicitudes);
            
        }
    }
}
