using Iglesia_Página_Web.Data;
using Iglesia_Página_Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Iglesia_Página_Web.Controllers
{
    public class NoticiasController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<HomeController> _logger;

        public NoticiasController(ILogger<HomeController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }
        [HttpGet]
        public async Task<IActionResult> NoticiaList()
        {
            return View(await _context.Noticias.ToListAsync());
        }
        [HttpGet]
        public IActionResult NoticiaCrear()
        {
            ViewBag.Today = DateTime.Now.ToString("yyyy-MM-dd");
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Noticia noticia)
        {
            if (ModelState.IsValid)
            {
                _context.Add(noticia);
                await _context.SaveChangesAsync();
                return RedirectToAction("NoticiaList");
            }
            return View(noticia);
        }
        public async Task<IActionResult> NoticiaEditar(int id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var noticia = await _context.Noticias.FindAsync(id);

            if (noticia == null)
            {
                return NotFound();
            }

            return View(noticia);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Noticia noticia)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(noticia);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!NoticiaExiste(noticia.NoticiaID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("NoticiaList");
            }
            return View(noticia);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var noticia = await _context.Noticias.FindAsync(id);
            _context.Noticias.Remove(noticia);
            await _context.SaveChangesAsync();
            return RedirectToAction("NoticiaList");
        }
        private bool NoticiaExiste(int id)
        {
            return _context.Noticias.Any(e => e.NoticiaID == id);
        }
    }
}
