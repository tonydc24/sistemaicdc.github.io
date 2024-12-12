using Microsoft.AspNetCore.Mvc;
using Iglesia_Página_Web.Data;
using Iglesia_Página_Web.Models;
using System.Linq;
using Iglesia_Página_Web.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;

namespace Iglesia_Página_Web.Controllers
{
    public class GamesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private static int nivelActual = 1; // Nivel inicial (1: Básico, 2: Medio, 3: Difícil)
        private static int preguntaIndex = 0; // Índice de la pregunta actual
        private static List<PreguntaTrivia> preguntasNivel; // Lista de preguntas del nivel actual
        private static List<string> respuestasMezcladas; // Respuestas mezcladas para la pregunta actual

        public GamesController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult GamesInicio()
        {
            return View();
        }

        public IActionResult TriviaIndex()
        {
            // Si ya no hay más preguntas en este nivel
            if (preguntasNivel == null || preguntaIndex >= preguntasNivel.Count)
            {
                preguntasNivel = _context.PreguntasTrivia
                    .Where(p => p.Nivel == ObtenerNivel()) 
                    .OrderBy(p => Guid.NewGuid()) 
                    .ToList();

                preguntaIndex = 0;

                if (preguntasNivel.Count == 0)
                {
                    return RedirectToAction("Resultado");
                }
            }

            // Obtener la pregunta actual
            var preguntaActual = preguntasNivel[preguntaIndex];

            if (respuestasMezcladas == null || respuestasMezcladas.Count == 0)
            {
                respuestasMezcladas = new List<string>
                {
                    preguntaActual.RespuestaCorrecta,
                    preguntaActual.RespuestaIncorrecta1,
                    preguntaActual.RespuestaIncorrecta2,
                    preguntaActual.RespuestaIncorrecta3
                }.OrderBy(r => Guid.NewGuid()).ToList();
            }

            ViewBag.Pregunta = preguntaActual;
            ViewBag.Respuestas = respuestasMezcladas;

            return View();
        }

        [HttpPost]
        public IActionResult Verificar(string respuestaSeleccionada)
        {
            var preguntaActual = preguntasNivel[preguntaIndex];

          
            if (respuestaSeleccionada == preguntaActual.RespuestaCorrecta)
            {
            
                if (preguntaIndex == preguntasNivel.Count - 1 && nivelActual < 3)
                {
                    nivelActual++;
                    preguntasNivel = null; 
                }
                else if (preguntaIndex == preguntasNivel.Count - 1 && nivelActual == 3)
                {
                    return RedirectToAction("Resultado"); 
                }
                else
                {
                    preguntaIndex++;
                }

                respuestasMezcladas = null; 
            }
            else
            {
                
                TempData["Mensaje"] = "Respuesta incorrecta. ¡Inténtalo nuevamente!";
            }

            return RedirectToAction("TriviaIndex");
        }

        public IActionResult Resultado()
        {
            ViewBag.Mensaje = "¡Felicidades! Has respondido todas las preguntas de la trivia. ¡Buen trabajo!";
            return View();
        }

        public IActionResult Reiniciar()
        {
            nivelActual = 1;
            preguntaIndex = 0;
            preguntasNivel = null;
            respuestasMezcladas = null;
            return RedirectToAction("TriviaIndex");
        }

        private string ObtenerNivel()
        {
            return nivelActual switch
            {
                1 => "Basico",
                2 => "Medio",
                3 => "Dificil",
                _ => "Basico"
            };
        }
        public IActionResult Preguntas(string nivel)
        {
            ViewData["Nivel"] = nivel ?? "Basico"; // Si no se selecciona nivel, usar "Basico" como predeterminado

            List<PreguntaTrivia> preguntas;

            if (string.IsNullOrEmpty(nivel) || nivel == "Basico" || nivel == "Medio" || nivel == "Dificil")
            {
                preguntas = _context.PreguntasTrivia.Where(p => p.Nivel == nivel).ToList();
            }
            else
            {
                preguntas = _context.PreguntasTrivia.ToList(); // Muestra todas las preguntas si no hay filtro
            }

            return View(preguntas);
        }
        [HttpGet]
        public IActionResult CrearPreguntas()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> CrearPreguntas(PreguntaTrivia pregunta)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(pregunta);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Preguntas));
                }
                catch (DbUpdateException)
                {
                    ModelState.AddModelError("", "No se pudo guardar la pregunta. Por favor, inténtelo de nuevo.");
                }
            }
            return View(pregunta);
        }

        [HttpGet]
        public IActionResult EditarPreguntas(int id)
        {
            var pregunta = _context.PreguntasTrivia.Find(id);
            if (pregunta == null)
            {
                return NotFound();
            }


            return View(pregunta);
        }


        // Acción para guardar los cambios de la pregunta editada
        [HttpPost]

        public async Task<IActionResult> EditarPreguntas(PreguntaTrivia item)
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
                    if (!ItemExiste(item.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Preguntas");
            }
            return View(item);
        }

    
        public async Task<IActionResult> EliminarPregunta(int id)
        {
            var item = await _context.PreguntasTrivia.FindAsync(id);
            _context.PreguntasTrivia.Remove(item);
            await _context.SaveChangesAsync();
            return RedirectToAction("Preguntas");
        }

        private bool ItemExiste(int id)
        {
            return _context.PreguntasTrivia.Any(e => e.Id == id);
        }



    }
}
