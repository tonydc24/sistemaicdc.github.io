using Iglesia_Página_Web.Data;
using Iglesia_Página_Web.Models;
using Iglesia_Página_Web.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;



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
        var logs = await _context.Logs.ToListAsync();

        var viewModel = new UsersViewModel
        {
            LoginsTodayCount = loginsTodayCount,
            Users = usersWithLastLogin,
            Logs = logs
        };

        return View(viewModel);
    }
    public IActionResult UserCrear()
    {
        return View();
    }
    [HttpPost]
    public async Task<IActionResult> UserCrear(RegistroViewModel model)
    {
        if (ModelState.IsValid)
        {
            // Verificar si ya existe un usuario con el mismo correo electrónico
            var usuarioExistentePorCorreo = await _context.Users
                .FirstOrDefaultAsync(u => u.CorreoElectrónico == model.CorreoElectrónico);

            // Verificar si ya existe un usuario con el mismo nombre de usuario
            var usuarioExistentePorNombre = await _context.Users
                .FirstOrDefaultAsync(u => u.NombreUsuario == model.NombreUsuario);

            if (usuarioExistentePorCorreo != null && usuarioExistentePorNombre != null)
            {
                // Si ambos existen, agregar un mensaje al ViewBag
                ViewBag.ErrorMessage = "El nombre de usuario y el correo electrónico ya están registrados.";
            }
            else if (usuarioExistentePorCorreo != null)
            {
                // Si solo el correo electrónico existe, agregar un mensaje al ViewBag
                ViewBag.ErrorMessage = "Este correo electrónico ya está registrado.";
            }
            else if (usuarioExistentePorNombre != null)
            {
                // Si solo el nombre de usuario existe, agregar un mensaje al ViewBag
                ViewBag.ErrorMessage = "Este nombre de usuario ya está registrado.";
            }

            // Si hay errores en el ModelState o en el ViewBag, retornar la vista con los errores
            if (!ModelState.IsValid || ViewBag.ErrorMessage != null)
            {
                return View(model);
            }

            // Si no hay errores, crear el nuevo usuario
            var user = new User
            {
                NombreUsuario = model.NombreUsuario,
                Contraseña = BCrypt.Net.BCrypt.HashPassword(model.Contraseña),
                CorreoElectrónico = model.CorreoElectrónico,
                Vigencia = DateTime.Now,
                RolID = model.RolID
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return RedirectToAction("Dashboard");
        }

        // Si el ModelState no es válido, retornar la vista con los errores
        foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
        {
            Debug.WriteLine($"Error: {error.ErrorMessage}");
        }
        ViewBag.ErrorMessage = "Error al realizar el registro!";
        return View(model);
    }

    [HttpGet]
    public async Task<IActionResult> UserEditar(int id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var user = await _context.Users.FindAsync(id);

        if (user == null)
        {
            return NotFound();
        }

        return View(user);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> UserEditar(User model)
    {
        // Verificar si el correo o el nombre de usuario ya existen
        var existingUserByUsername = await _context.Users
            .Where(u => u.NombreUsuario == model.NombreUsuario && u.UsuarioID != model.UsuarioID)
            .FirstOrDefaultAsync();

        var existingUserByEmail = await _context.Users
            .Where(u => u.CorreoElectrónico == model.CorreoElectrónico && u.UsuarioID != model.UsuarioID)
            .FirstOrDefaultAsync();

        if (existingUserByUsername != null && existingUserByEmail != null)
        {
            ViewBag.ErrorMessage = "El nombre de usuario y el correo electrónico ya están en uso.";
            return View(model);
        }
        else if (existingUserByUsername != null)
        {
            ViewBag.ErrorMessage = "El nombre de usuario ya está en uso.";
            return View(model);
        }
        else if (existingUserByEmail != null)
        {
            ViewBag.ErrorMessage = "El correo electrónico ya está en uso.";
            return View(model);
        }

        // Actualizar usuario
        try
        {
            var user = await _context.Users.FindAsync(model.UsuarioID);
            if (user == null)
            {
                return NotFound();
            }

            user.NombreUsuario = model.NombreUsuario;
            user.CorreoElectrónico = model.CorreoElectrónico;
            user.RolID = model.RolID;

        

            _context.Update(user);
            await _context.SaveChangesAsync();
            return RedirectToAction("Dashboard");
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!UserExiste(model.UsuarioID))
            {
                return NotFound();
            }
            else
            {
                throw;
            }
        }
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

    private bool UserExiste(int id)
    {
        return _context.Users.Any(e => e.UsuarioID == id);
    }

}
