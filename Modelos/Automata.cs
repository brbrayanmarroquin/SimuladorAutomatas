using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;

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
            if (estado == null)
            {
                throw new ArgumentNullException(nameof(estado));
            }

            // No permitimos dos estados con el mismo nombre
            bool nombreExiste = Estados.Any(e =>
                e.Nombre.Equals(estado.Nombre, StringComparison.OrdinalIgnoreCase));

            if (!nombreExiste)
            {
                Estados.Add(estado);
            }
        }

        public void EstablecerEstadoInicial(Estado estado)
        {
            if (estado == null)
            {
                throw new ArgumentNullException(nameof(estado));
            }

            if (!Estados.Contains(estado))
            {
                AgregarEstado(estado);
            }

            // Quitamos la marca de inicial al estado anterior
            if (EstadoInicial != null)
            {
                EstadoInicial.EsInicial = false;
            }

            EstadoInicial = estado;
            estado.EsInicial = true;
        }

        public void AgregarEstadoFinal(Estado estado)
        {
            if (estado == null)
            {
                throw new ArgumentNullException(nameof(estado));
            }

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

        public void AgregarTransicion(
            Estado origen,
            string simbolo,
            Estado destino)
        {
            if (origen == null)
            {
                throw new ArgumentNullException(nameof(origen));
            }

            if (destino == null)
            {
                throw new ArgumentNullException(nameof(destino));
            }

            if (string.IsNullOrWhiteSpace(simbolo))
            {
                throw new ArgumentException(
                    "El símbolo de la transición no puede estar vacío.",
                    nameof(simbolo));
            }

            if (!Estados.Contains(origen))
            {
                AgregarEstado(origen);
            }

            if (!Estados.Contains(destino))
            {
                AgregarEstado(destino);
            }

            Transicion transicion =
                new Transicion(origen, simbolo, destino);

            Transiciones.Add(transicion);

            // ε no pertenece al alfabeto
            if (simbolo != "ε")
            {
                Alfabeto.Add(simbolo);
            }
        }
    }
}