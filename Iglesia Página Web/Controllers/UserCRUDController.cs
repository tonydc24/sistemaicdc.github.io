using Iglesia_Página_Web.Data;
using Iglesia_Página_Web.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;



public class UserCRUDController : Controller
{
    private readonly ApplicationDbContext _context;

    public UserCRUDController(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Dashboard()
    {
        var today = DateTime.Today;

        // Conteo de inicios de sesion de hoy
        var loginsTodayCount = await _context.Logs
        .Where(log => log.Accion == "Login" && log.FechaHora.Date == today)
        .GroupBy(log => log.UsuarioID)
        .CountAsync();

        // Se obtiene el usuario y su ultimo login  
        var usersWithLastLogin = await _context.Users
            .Include(u => u.Rols)
            .Select(u => new UserWithLastLogin
            {
                UsuarioID = u.UsuarioID,
                NombreUsuario = u.NombreUsuario,
                Email = u.CorreoElectrónico,
                NombreRol = u.Rols.NombreRol,
                LastLogin = _context.Logs
                    .Where(log => log.UsuarioID == u.UsuarioID && log.Accion == "Login")
                    .OrderByDescending(log => log.FechaHora)
                    .Select(log => (DateTime?)log.FechaHora)
                    .FirstOrDefault()
            })
            .ToListAsync();

        var viewModel = new UsersViewModel
        {
            LoginsTodayCount = loginsTodayCount,
            Users = usersWithLastLogin
        };

        return View(viewModel);
    }
    public async Task<IActionResult> Delete(int id)
    {
        var usuario = await _context.Users.FindAsync(id);

        if (usuario == null)
        {
            return NotFound();
        }

        var logs = _context.Logs.Where(l => l.UsuarioID == id);
        _context.Logs.RemoveRange(logs);

        _context.Users.Remove(usuario);
        await _context.SaveChangesAsync();

        return RedirectToAction("Dashboard");
    }

}
