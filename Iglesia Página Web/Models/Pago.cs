using System;

namespace Iglesia_Página_Web.Models
{
    public class Pago
    {
        public int PagoId { get; set; }
        public string FacturaId { get; set; }
        public DateTime FechaPago { get; set; }
        public decimal MontoPagado { get; set; }
        public string MetodoPago { get; set; }
    }
}
