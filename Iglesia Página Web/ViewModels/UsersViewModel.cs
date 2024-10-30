using Iglesia_Página_Web.Models;

namespace Iglesia_Página_Web.ViewModels
{
    public class UsersViewModel
    {
        public int LoginsTodayCount { get; set; }
        public List<UserWithLastLogin> Users { get; set; }
    }

    public class UserWithLastLogin
    {
        public int UsuarioID { get; set; }
        public string NombreUsuario { get; set; }
        public string Email { get; set; }
        public string NombreRol { get; set; }
        public DateTime? LastLogin { get; set; }
    }

}
