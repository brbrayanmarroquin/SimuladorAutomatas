using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace SimuladorAutomatas.Modelos
{
    public class RegistroValidacion
    {
        public DateTime FechaHora { get; set; }

        public string Cadena { get; set; }

        public string TipoAutomata { get; set; }

        public bool Aceptada { get; set; }

        public string Resultado
        {
            get
            {
                return Aceptada ? "Aceptada" : "Rechazada";
            }
        }

        public RegistroValidacion(
            string cadena,
            string tipoAutomata,
            bool aceptada)
        {
            FechaHora = DateTime.Now;
            Cadena = cadena;
            TipoAutomata = tipoAutomata;
            Aceptada = aceptada;
        }
    }
}

