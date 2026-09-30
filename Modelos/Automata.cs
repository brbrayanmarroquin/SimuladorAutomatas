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

        public void AgregarEstado(Estado estado)
        {
            if (!Estados.Contains(estado))
            {
                Estados.Add(estado);
            }
        }

        public void EstablecerEstadoInicial(Estado estado)
        {
            if (!Estados.Contains(estado))
            {
                AgregarEstado(estado);
            }

            EstadoInicial = estado;
            estado.EsInicial = true;
        }

        public void AgregarEstadoFinal(Estado estado)
        {
            if (!Estados.Contains(estado))
            {
                AgregarEstado(estado);
            }

            if (!EstadosFinales.Contains(estado))
            {
                EstadosFinales.Add(estado);
            }

            estado.EsFinal = true;
        }

        public void AgregarTransicion(Estado origen, string simbolo, Estado destino)
        {
            if (!Estados.Contains(origen))
            {
                AgregarEstado(origen);
            }

            if (!Estados.Contains(destino))
            {
                AgregarEstado(destino);
            }

            Transicion transicion = new Transicion(origen, simbolo, destino);

            Transiciones.Add(transicion);

            if (simbolo != "ε")
            {
                Alfabeto.Add(simbolo);
            }
        }
    }
}