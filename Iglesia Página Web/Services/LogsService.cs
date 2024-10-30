using Iglesia_Página_Web.Models;
using Iglesia_Página_Web.ViewModels;
using Iglesia_Página_Web.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace Iglesia_Página_Web.Services
{
    public class LogsService : ILogsService
    {
        private readonly ApplicationDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public LogsService(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task LogAsync(string action)
        {
            try
            {
                var userIdClaim = _httpContextAccessor.HttpContext?.User?.FindFirst("UserID");
                if (userIdClaim == null)
                {
                    throw new Exception("User is not authenticated.");
                }

                var userId = int.Parse(userIdClaim.Value);

                var log = new Log
                {
                    UsuarioID = userId,
                    Accion = action,
                };

                _context.Logs.Add(log);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
            
                throw new Exception("Error al registrar el log", ex);
            }
        }
    }
}
