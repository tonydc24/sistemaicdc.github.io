using System;

namespace Iglesia_Página_Web.Models
{
    public class Noticia
    {
        public int NoticiaID { get; set; } 
        public string Titulo { get; set; } 
        public string Contenido { get; set; } 
        public DateTime FechaPublicacion { get; set; } 
        public int AdministradorID { get; set; } 
    }

}
