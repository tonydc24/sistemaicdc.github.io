using Iglesia_Página_Web.Models;
using Microsoft.EntityFrameworkCore;

namespace Iglesia_Página_Web.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    public DbSet<Rol> Roles { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet <Noticia> Noticias { get; set; }
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer("Server=localhost\\SQLEXPRESS;Database=SistemaWeb;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True");
    }
}
