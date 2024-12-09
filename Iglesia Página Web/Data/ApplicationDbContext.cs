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
     
        optionsBuilder.UseSqlServer("Server=localhost\\SQLEXPRESS01;Database=SistemaWeb;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True");
      
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<PreguntaTrivia>().HasNoKey(); 
        base.OnModelCreating(modelBuilder);
    }

}
