using Iglesia_Página_Web.Models;
using Microsoft.EntityFrameworkCore;

namespace Iglesia_Página_Web.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<User> Usuarios { get; set; }
        public DbSet <Noticia> Noticias { get; set; }
    }
}
