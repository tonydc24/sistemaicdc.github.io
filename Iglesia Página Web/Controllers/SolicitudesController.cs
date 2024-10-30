using Iglesia_Página_Web.Data;
using Iglesia_Página_Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Security.Claims; 

[Authorize] 
public class SolicitudesController : Controller
{
    private readonly ApplicationDbContext _context;

    public SolicitudesController(ApplicationDbContext context)
    {
        _context = context;
    }
  
    [HttpGet]
    public async Task<IActionResult> SolicitudesInicio()
    {
        return View(await _context.Solicitudes.ToListAsync());
    }

    [HttpPost]
    public IActionResult CrearSolicitud(string DetalleSolicitud)
    {
        var usuarioId = GetCurrentUserId();

        if (usuarioId == null)
        {
            return BadRequest("No se pudo encontrar el ID de usuario.");
        }

        var nuevaSolicitud = new Solicitud
        {
            UsuarioID = usuarioId.Value, 
            DetalleSolicitud = DetalleSolicitud,
            Estado = "Pendiente", 
            FechaSolicitud = DateTime.Now
        };

        _context.Solicitudes.Add(nuevaSolicitud);
        _context.SaveChanges();

        
        return RedirectToAction("InventarioInicio" , "Home"); 
    }

    private int? GetCurrentUserId()
    {

        var claim = User.FindFirst("UserID");
        return claim != null ? (int?)int.Parse(claim.Value) : null; 
    }
}
