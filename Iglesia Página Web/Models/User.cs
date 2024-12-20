﻿using System.ComponentModel.DataAnnotations;

namespace Iglesia_Página_Web.Models
{
    public class User
    {
        [Key]
        public int UsuarioID { get; set; }

        [Required(ErrorMessage = "El nombre de usuario es obligatorio.")]
        public string NombreUsuario { get; set; }


        public string Contraseña { get; set; }

        [Required(ErrorMessage = "El correo electrónico es obligatorio.")]
        [EmailAddress(ErrorMessage = "El correo electrónico no es válido.")]
        public string CorreoElectrónico { get; set; }

        public int RolID { get; set; }
        public bool ClaveTemp { get; set; }
        public DateTime Vigencia { get; set; }


        public virtual Rol Rols { get; set; }
	}

}
