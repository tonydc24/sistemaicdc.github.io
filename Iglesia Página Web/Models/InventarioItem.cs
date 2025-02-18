using System.ComponentModel.DataAnnotations;

namespace Iglesia_Página_Web.Models
{
    public class InventarioItem
    {
        [Key]
        public int ArticuloID { get; set; }
        public int Responsable { get; set; }

        public string NombreArticulo { get; set; }
        public string Descripcion { get; set; }
        
        public int Cantidad { get; set; }

        public DateTime FechaDeCreacion { get; set; }

        public InventarioItem()
        {
            FechaDeCreacion = DateTime.Now;
        }
    }
}
