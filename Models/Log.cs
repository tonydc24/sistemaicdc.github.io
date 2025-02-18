namespace Iglesia_Página_Web.Models
{
    public class Log
    {
        public int LogID { get; set; }
        public int UsuarioID { get; set; }
        public string Accion { get; set; }

        public DateTime FechaHora { get; set; }

        public Log () {
            FechaHora = DateTime.Now;
        }
    }
}
