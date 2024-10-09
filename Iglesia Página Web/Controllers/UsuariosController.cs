using Microsoft.AspNetCore.Mvc;
using Iglesia_Página_Web.Models;
using Iglesia_Página_Web.ViewModels;
using Iglesia_Página_Web.Data;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using System.Diagnostics;
using Microsoft.EntityFrameworkCore;

namespace Iglesia_Página_Web.Controllers
{
	public class UsuariosController : Controller
	{

		private readonly ApplicationDbContext _context;

		public UsuariosController(ApplicationDbContext context)
		{
			_context = context;
		}
		//[HttpGet]
		//public IActionResult Usuario()
		//{
		//	var usuarios = _context.Usuario.ToList();
		//	return View(usuarios);
		//}
	

        public IActionResult Register()
		{
			return View();
		}


        [HttpPost]
        public async Task<IActionResult> Register(RegistroViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new User
                {
                    NombreUsuario = model.NombreUsuario,
                    Contraseña = BCrypt.Net.BCrypt.HashPassword(model.Contraseña),
                    CorreoElectrónico = model.CorreoElectrónico,
                    RolID = model.RolID 
                };

                _context.Users.Add(user);
                await _context.SaveChangesAsync();
                return RedirectToAction("Login");
            }
            foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
            {
                Debug.WriteLine($"Error: {error.ErrorMessage}"); 
            }
            ViewBag.ErrorMessage = "Error al realizar el registro!";
            return View(model);
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        public async Task<IActionResult> Login(string NombreUsuario, string Contraseña)
        {
            // Comprobar si se proporcionaron datos
            if (string.IsNullOrEmpty(NombreUsuario) || string.IsNullOrEmpty(Contraseña))
            {
                ViewBag.ErrorMessage = "Por favor ingresa el nombre de usuario y la contraseña.";
                return View();
            }

            // Buscar el usuario en la base de datos, incluyendo el rol
            var user = await _context.Users
                .Include(u => u.Rols) 
                .FirstOrDefaultAsync(u => u.NombreUsuario == NombreUsuario);

            // Comprobar si el usuario existe
            if (user == null)
            {
                ViewBag.ErrorMessage = "Nombre de usuario no encontrado.";
                return View();
            }

            // Verificar la contraseña
            if (!BCrypt.Net.BCrypt.Verify(Contraseña, user.Contraseña))
            {
                ViewBag.ErrorMessage = "Contraseña incorrecta.";
                return View();
            }

            var claims = new List<Claim>
             {
                new Claim(ClaimTypes.Name, user.NombreUsuario),
                new Claim(ClaimTypes.Role, user.Rols.NombreRol)
             };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));

            return RedirectToAction("Index", "Home");
        }

        //[HttpPost]
        //public IActionResult Editar(int id, User usuarioActualizado)
        //{
        //    return RedirectToAction("Index");
        //}


        //[HttpPost]

        //public IActionResult Eliminar(int id)
        //{
        //    return RedirectToAction("Index");
        //}
    }
}
