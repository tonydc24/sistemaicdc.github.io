namespace Iglesia_Página_Web.Models
{
    public class Solicitud
    {
        public string SolicitudId { get; set; }
        public string UsuarioId { get; set; }
        public string DetalleSolicitud { get; set; }
        public string Estado {  get; set; }
        public DateTime FechaSolicitud { get; set; }
    }
}
