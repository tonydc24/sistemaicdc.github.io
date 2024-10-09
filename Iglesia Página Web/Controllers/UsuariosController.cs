using Microsoft.AspNetCore.Mvc;
using Iglesia_Página_Web.Models;
using Iglesia_Página_Web.Data;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;

namespace Iglesia_Página_Web.Controllers
{
    public class UsuariosController : Controller
    {

        private readonly ApplicationDbContext _context;

        public UsuariosController(ApplicationDbContext context)
        {
            _context = context;
        }
        [HttpGet]
        public IActionResult Usuario()
        {
            var usuarios = _context.Usuarios.ToList();
            return View(usuarios);
        }
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> Login(string correo, string password)
        {
            var user = _context.Usuarios.FirstOrDefault(u => u.CorreoElectrónico == correo);

            if (user != null && BCrypt.Net.BCrypt.Verify(password, user.Contraseña))
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.CorreoElectrónico),
                    new Claim(ClaimTypes.Role, user.Rol.NombreRol)
                };

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));

                return RedirectToAction("Index", "Home");
            }

            ViewBag.ErrorMessage = "Invalid credentials";
            return View();
        }

        public IActionResult Register()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Register(User model)
        {
            if (ModelState.IsValid)
            {
                model.Contraseña = BCrypt.Net.BCrypt.HashPassword(model.Contraseña);
                _context.Usuarios.Add(model);
                await _context.SaveChangesAsync();
                return RedirectToAction("Login");
            }
            return View(model);
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
