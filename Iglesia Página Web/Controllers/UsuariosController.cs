using Microsoft.AspNetCore.Mvc;
using Iglesia_Página_Web.Models;
using Iglesia_Página_Web.ViewModels;
using Iglesia_Página_Web.Data;
using Iglesia_Página_Web.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using System.Diagnostics;
using Microsoft.EntityFrameworkCore;
using System.Drawing;
using System.Net.Mail;
using System.Text;
using System;

namespace Iglesia_Página_Web.Controllers
{
	public class UsuariosController : Controller
	{

		private readonly ApplicationDbContext _context;
        private readonly IConfiguration _conf;
        private readonly IHostEnvironment _env;
        private readonly ILogsService _logsService;

        public UsuariosController(ApplicationDbContext context , IConfiguration conf, IHostEnvironment env , ILogsService logs)
        {
            _context = context;
            _conf = conf;
            _env = env;
            _logsService = logs;
        }
        //[HttpGet]
        //public IActionResult Usuario()
        //{
        //	var usuarios = _context.Usuario.ToList();
        //	return View(usuarios);
        //}

        public async Task<IActionResult> UsuariosInicio()
        {
            return View(await _context.Users.ToListAsync());
        }
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
                    Vigencia = DateTime.Now,
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
            if (user.ClaveTemp && user.Vigencia < DateTime.Now)
            {
                ViewBag.ErrorMessage = "Su contraseña temporal de acceso ha expirado.";
                return View();
       
            }
            var claims = new List<Claim>
            {
                new Claim("UserID", user.UsuarioID.ToString()),
                new Claim(ClaimTypes.Name, user.NombreUsuario),
                new Claim(ClaimTypes.Role, user.Rols.NombreRol)
            };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var authProperties = new AuthenticationProperties
            {
                IsPersistent = true,
                ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(80)
            };

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                authProperties);
            //Registro del log 
         
            if (user.ClaveTemp == true && user.Vigencia > DateTime.Now)
            {
                return RedirectToAction("ChangePassword");
            }

            return RedirectToAction("Dashboard");
        }
        public async Task <IActionResult> Dashboard()
        {
            if (User.Identity.IsAuthenticated)
            {
                await _logsService.LogAsync("Login");
            }
            return RedirectToAction("Index", "Home");
        }
        public async Task<IActionResult> Logout()
        {
            await _logsService.LogAsync("LogOut");
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login");
        }
        [HttpGet]
        public IActionResult RecoverPassword() { 
            return View (); 
        }
        [HttpPost]
        public async Task<IActionResult> RecoverPassword (User model)
        {
            var result = await _context.Users
                .Include(u => u.Rols)
                .FirstOrDefaultAsync(u => u.CorreoElectrónico == model.CorreoElectrónico);
            if (result != null)
            {
                var Codigo = GenerarCodigo();
                var Password = BCrypt.Net.BCrypt.HashPassword(Codigo);
                var ClaveTemp = true;
                var Vigencia = DateTime.Now.AddMinutes(15);

                result.Contraseña = Password;
                result.ClaveTemp = ClaveTemp;
                result.Vigencia = Vigencia;


                _context.Entry(result).State = EntityState.Modified;
                await _context.SaveChangesAsync();

                var ruta = Path.Combine(_env.ContentRootPath, "Views/Usuarios/RecuperarAcceso.html");
                var html = System.IO.File.ReadAllText(ruta);

                html = html.Replace("@@Nombre", result.NombreUsuario);
                html = html.Replace("@@Contrasenna", Codigo);
                html = html.Replace("@@Vencimiento", Vigencia.ToString("dd/MM/yyyy hh:mm tt"));

                EnviarCorreo(result.CorreoElectrónico, "Recuperar Accesos Sistema", html);
                return RedirectToAction("Login");



            }

            else
            {
                ViewBag.ErrorMessage = "Correo no encontrado.";
                return View();
            }
            return RedirectToAction("Login");
        }

        [HttpGet]
        public IActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ChangePassword(int ID, string Password, string ConfirmPassword)
        {
            if (User.Identity.IsAuthenticated)
            {
                if (Password == ConfirmPassword)
                {
           
                    var user = await _context.Users.FindAsync(ID);
                    if (user != null)
                    {
               
                        var hashedPassword = BCrypt.Net.BCrypt.HashPassword(Password);
                        var ClaveTemp = false;

                        user.ClaveTemp = ClaveTemp;
                        user.Contraseña = hashedPassword;

                      
                        _context.Entry(user).State = EntityState.Modified;
                        await _context.SaveChangesAsync();

                        ViewBag.SuccessMessage = "Contraseña cambiada exitosamente.";
                        await _logsService.LogAsync("Cambio de contraseña");
                        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                        return RedirectToAction("Login");
                    }
                    else
                    {
                        ViewBag.ErrorMessage = "Usuario no encontrado.";
                        return View();
                    }
                }
                else
                {
                    ViewBag.ErrorMessage = "Las contraseñas no coinciden.";
                    return View();
                }
            }
            else
            {
                ViewBag.ErrorMessage = "Necesita estar con su sesión iniciada para usar esta función.";
                return View();
            }
        }

        private void EnviarCorreo(string destino, string asunto, string contenido)
        {
            string cuenta = _conf.GetSection("Variables:CorreoEmail").Value!;
            string contrasenna = _conf.GetSection("Variables:ClaveEmail").Value!;

            MailMessage message = new MailMessage();
            message.From = new MailAddress(cuenta);
            message.To.Add(new MailAddress(destino));
            message.Subject = asunto;
            message.Body = contenido;
            message.Priority = MailPriority.Normal;
            message.IsBodyHtml = true;

            SmtpClient client = new SmtpClient("smtp.gmail.com", 587);
            client.Credentials = new System.Net.NetworkCredential(cuenta, contrasenna);
            client.EnableSsl = true;


            //Esto es para que no se intente enviar el correo si no hay una contraseña
            if (!string.IsNullOrEmpty(contrasenna))
            {
                client.Send(message);
            }

        }
        private string GenerarCodigo()
        {
            int length = 8;
            const string valid = "ABCDEFGHIJKLMNOPQRSTUVWXYZ012456789";
            StringBuilder res = new StringBuilder();
            Random rnd = new Random();
            while (0 < length--)
            {
                res.Append(valid[rnd.Next(valid.Length)]);
            }
            return res.ToString();
        }
    }
}
