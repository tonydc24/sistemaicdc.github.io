using Iglesia_Página_Web.Models;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;

namespace Iglesia_Página_Web.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
    public DbSet<Log> Logs { get; set; }
    public DbSet<Rol> Roles { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet <Noticia> Noticias { get; set; }
    public DbSet <Solicitud> Solicitudes { get; set; }
    public DbSet <InventarioItem> Inventario { get; set; }
    public DbSet<PreguntaTrivia> PreguntasTrivia { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
     
        optionsBuilder.UseSqlServer("Server=sistemawebicdc.database.windows.net;Database=bdsistemaweb;User Id=userweb;Password=Password123#;MultipleActiveResultSets=true;TrustServerCertificate=True");
      
    }

  

}
