using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Iglesia_Página_Web.Models;

namespace Iglesia_Página_Web.Controllers
{
    public class InventarioController : Controller
    {
        public IActionResult Inventario()
        {
            var items = new List<InventarioItem>
            {
                new InventarioItem { Id = 1, Nombre = "Sillas", Cantidad = 2, Fecha = "1-1-2024" },
                new InventarioItem { Id = 2, Nombre = "Guitarras", Cantidad = 3, Fecha = "1-1-2024" },
                new InventarioItem { Id = 3, Nombre = "Micrófonos", Cantidad = 10, Fecha = "1-1-2024" }
            };

            return View(items);
        }

        public IActionResult Edit(int id)
        {
            var item = new InventarioItem { Id = id, Nombre = "Sillas", Cantidad = 2, Fecha = "1-1-2024" };

            return View(item);
        }

        public IActionResult Delete(int id)
        {
            return RedirectToAction("Index");
        }
    }
}
