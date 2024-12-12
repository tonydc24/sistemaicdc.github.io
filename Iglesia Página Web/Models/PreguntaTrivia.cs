namespace Iglesia_Página_Web.Models
{
    public class PreguntaTrivia
    {
        public int Id { get; set; }
        public string Pregunta { get; set; }
        public string RespuestaCorrecta { get; set; }
        public string RespuestaIncorrecta1 { get; set; }
        public string RespuestaIncorrecta2 { get; set; }
        public string RespuestaIncorrecta3 { get; set; }

        public string Nivel { get; set; }
    }
}
