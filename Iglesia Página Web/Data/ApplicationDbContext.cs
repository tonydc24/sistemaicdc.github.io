using Iglesia_Página_Web.Models;
using Microsoft.EntityFrameworkCore;

namespace Iglesia_Página_Web.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    public DbSet<User> Usuario { get; set; }
    public DbSet <Noticia> Noticias { get; set; }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer("Server=localhost\\SQLEXPRESS01;Database=SistemaWeb;Trusted_Connection=True;TrustServerCertificate=True");
    }
}
