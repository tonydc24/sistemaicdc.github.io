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
        public IActionResult Pago()
        {
            var pagos = new List<Pago>
            {
                new Pago { PagoId = 1, FacturaId = "F12345", FechaPago = new DateTime(2024, 8, 15), MontoPagado = 100.00M, MetodoPago = "Tarjeta de Crédito" },
                new Pago { PagoId = 2, FacturaId = "F12346", FechaPago = new DateTime(2024, 8, 16), MontoPagado = 150.00M, MetodoPago = "PayPal" },
                new Pago { PagoId = 3, FacturaId = "F12347", FechaPago = new DateTime(2024, 8, 17), MontoPagado = 200.00M, MetodoPago = "Transferencia Bancaria" }
            };

            return View(pagos);
        }
    }
}
