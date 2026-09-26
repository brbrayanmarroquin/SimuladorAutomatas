using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimuladorAutomatas.Modelos
{
    public class Estado
    {
        public string Nombre { get; set; }

        public bool EsInicial { get; set; }

        public bool EsFinal { get; set; }

        public Estado(string nombre, bool esInicial = false, bool esFinal = false)
        {
            Nombre = nombre;
            EsInicial = esInicial;
            EsFinal = esFinal;
        }

        public override string ToString()
        {
            return Nombre;
        }
    }
}