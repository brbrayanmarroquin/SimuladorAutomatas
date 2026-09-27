using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimuladorAutomatas.Modelos
{
    public class Transicion
    {
        public Estado EstadoOrigen { get; set; }

        public string Simbolo { get; set; }

        public Estado EstadoDestino { get; set; }

        public Transicion(Estado estadoOrigen, string simbolo, Estado estadoDestino)
        {
            EstadoOrigen = estadoOrigen;
            Simbolo = simbolo;
            EstadoDestino = estadoDestino;
        }

        public override string ToString()
        {
            return $"{EstadoOrigen} --{Simbolo}--> {EstadoDestino}";
        }
    }
}