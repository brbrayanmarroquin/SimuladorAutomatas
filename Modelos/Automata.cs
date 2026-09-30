using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace SimuladorAutomatas.Modelos
{
    public class Automata
    {
        public List<Estado> Estados { get; set; }

        public Estado EstadoInicial { get; set; }

        public List<Estado> EstadosFinales { get; set; }

        public List<Transicion> Transiciones { get; set; }

        public HashSet<string> Alfabeto { get; set; }

        public Automata()
        {
            Estados = new List<Estado>();
            EstadosFinales = new List<Estado>();
            Transiciones = new List<Transicion>();
            Alfabeto = new HashSet<string>();
        }
    }
}