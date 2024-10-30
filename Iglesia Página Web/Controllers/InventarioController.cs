using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Iglesia_Página_Web.Models;
using Microsoft.EntityFrameworkCore;
using System;
using Iglesia_Página_Web.Data;

namespace Iglesia_Página_Web.Controllers
{
    public class InventarioController : Controller
    {
    
        private readonly ApplicationDbContext _context;

        public InventarioController(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> InventarioInicio()
        {
            return View(await _context.Inventario.ToListAsync());
        }

        public async Task<IActionResult> ItemDetalles(int id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var item = await _context.Inventario.FirstOrDefaultAsync(m => m.ArticuloID == id);

            if (item == null)
            {
                return NotFound();
            }

            return View(item);
        }
        //GET Create
        public IActionResult ItemCrear()
        {
            return View();
        }
        //POST Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ItemCrear(InventarioItem item)
        {
             
            if (ModelState.IsValid)
            {
                _context.Add(item);
                await _context.SaveChangesAsync();
                return RedirectToAction("InventarioInicio");
            }
            return View(item);
        }
        [HttpGet]
        public async Task<IActionResult> ItemEditar(int id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var item = await _context.Inventario.FindAsync(id);

            if (item == null)
            {
                return NotFound();
            }

            return View(item);
        }
        //POST Edit Card
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ItemEditar(InventarioItem item)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(item);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VideogameExiste(item.ArticuloID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("InventarioInicio");
            }
            return View(item);
        }

        // Delete
        public async Task<IActionResult> Delete(int id)
        {
            var item = await _context.Inventario.FindAsync(id);
            _context.Inventario.Remove(item);
            await _context.SaveChangesAsync();
            return RedirectToAction("InventarioInicio");
        }

        private bool VideogameExiste(int id)
        {
            return _context.Inventario.Any(e => e.ArticuloID == id);
        }
    }
}
