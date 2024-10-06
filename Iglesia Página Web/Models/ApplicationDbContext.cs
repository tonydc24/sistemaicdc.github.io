using Microsoft.EntityFrameworkCore;

namespace Iglesia_Página_Web.Models
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)  { }

        public DbSet<Usuario> Usuarios { get; set; }
    }
}
