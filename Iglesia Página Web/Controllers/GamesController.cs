using Microsoft.AspNetCore.Mvc;
using Iglesia_Página_Web.Data;
using Iglesia_Página_Web.Models;
using System.Linq;
using Iglesia_Página_Web.Data;

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
    }
}
