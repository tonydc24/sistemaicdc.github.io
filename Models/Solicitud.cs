namespace Iglesia_Página_Web.Models
{
    public class Solicitud
    {
        public int SolicitudID { get; set; }
        public int UsuarioID { get; set; }
        public string DetalleSolicitud { get; set; }
        public string Estado {  get; set; }
        public DateTime FechaSolicitud { get; set; }
    }
}
