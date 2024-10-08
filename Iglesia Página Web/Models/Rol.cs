namespace Iglesia_Página_Web.Models
{
    public class Rol
    {
        public int RolID { get; set; }
        public string NombreRol { get; set; }

        public ICollection<Usuario> Usuarios { get; set; }
    }

}
