using System;
using System.Collections.Generic;
using System.Linq;
using SimuladorAutomatas.Modelos;

namespace SimuladorAutomatas.Servicios
{
    /// <summary>
    /// Contiene el resultado de una simulación y los pasos que se mostraron
    /// durante el procesamiento de la cadena.
    /// </summary>
    public class ResultadoSimulacion
    {
        public bool Aceptada { get; set; }
        public List<string> Pasos { get; set; } = new List<string>();
    }

    public class Simulador
    {
        // Se conserva este método para no romper las partes del proyecto
        // que solamente necesitan saber si una cadena se acepta o rechaza.
        public bool SimularAFN(Automata automata, string cadena)
        {
            return SimularConRecorrido(automata, cadena).Aceptada;
        }

        /// <summary>
        /// Simula la cadena y devuelve el conjunto de estados posibles
        /// después de cada símbolo, incluyendo las cerraduras epsilon.
        /// Esto funciona para AFN y también para AFD.
        /// </summary>
        public ResultadoSimulacion SimularConRecorrido(
            Automata automata,
            string cadena)
        {
            if (automata == null)
                throw new ArgumentNullException(nameof(automata));

            if (cadena == null)
                throw new ArgumentNullException(nameof(cadena));

            if (automata.EstadoInicial == null)
                throw new InvalidOperationException(
                    "El autómata no tiene un estado inicial.");

            ResultadoSimulacion resultado = new ResultadoSimulacion();

            HashSet<Estado> estadosActuales =
                new HashSet<Estado> { automata.EstadoInicial };

            estadosActuales = CerraduraEpsilon(automata, estadosActuales);

            resultado.Pasos.Add(
                "Inicio: " + FormatearEstados(estadosActuales));

            if (!estadosActuales.SetEquals(
                new HashSet<Estado> { automata.EstadoInicial }))
            {
                resultado.Pasos.Add(
                    "Cerradura ε inicial: " + FormatearEstados(estadosActuales));
            }

            int numeroPaso = 0;

            foreach (char caracter in cadena)
            {
                numeroPaso++;
                string simbolo = caracter.ToString();
                HashSet<Estado> estadosOrigen = new HashSet<Estado>(estadosActuales);
                HashSet<Estado> nuevosEstados = new HashSet<Estado>();

                foreach (Estado estadoActual in estadosActuales)
                {
                    List<Transicion> transiciones = automata.Transiciones
                        .Where(t => t.EstadoOrigen == estadoActual &&
                                    t.Simbolo == simbolo)
                        .ToList();

                    foreach (Transicion transicion in transiciones)
                    {
                        nuevosEstados.Add(transicion.EstadoDestino);
                    }
                }

                if (nuevosEstados.Count == 0)
                {
                    resultado.Pasos.Add(
                        "Paso " + numeroPaso + ": leer '" + simbolo +
                        "' desde " + FormatearEstados(estadosOrigen) +
                        " → ∅ (no existe una transición para este símbolo).");

                    estadosActuales.Clear();
                    break;
                }

                resultado.Pasos.Add(
                    "Paso " + numeroPaso + ": leer '" + simbolo +
                    "' desde " + FormatearEstados(estadosOrigen) +
                    " → " + FormatearEstados(nuevosEstados));

                HashSet<Estado> estadosConEpsilon =
                    CerraduraEpsilon(automata, nuevosEstados);

                if (!estadosConEpsilon.SetEquals(nuevosEstados))
                {
                    resultado.Pasos.Add(
                        "  Después de ε: " +
                        FormatearEstados(estadosConEpsilon));
                }

                estadosActuales = estadosConEpsilon;
            }

            bool hayEstadoFinal = estadosActuales.Any(estado =>
                estado.EsFinal ||
                (automata.EstadosFinales != null &&
                 automata.EstadosFinales.Contains(estado)));

            resultado.Aceptada = hayEstadoFinal;

            resultado.Pasos.Add(
                "Estados al terminar: " + FormatearEstados(estadosActuales));

            resultado.Pasos.Add(
                hayEstadoFinal
                    ? "Conclusión: se alcanzó al menos un estado final."
                    : "Conclusión: no se alcanzó ningún estado final.");

            return resultado;
        }

        private HashSet<Estado> CerraduraEpsilon(
            Automata automata,
            HashSet<Estado> estadosIniciales)
        {
            HashSet<Estado> resultado =
                new HashSet<Estado>(estadosIniciales);

            Stack<Estado> pendientes =
                new Stack<Estado>(estadosIniciales);

            while (pendientes.Count > 0)
            {
                Estado estadoActual = pendientes.Pop();

                List<Transicion> transicionesEpsilon =
                    automata.Transiciones
                        .Where(t => t.EstadoOrigen == estadoActual &&
                                    t.Simbolo == "ε")
                        .ToList();

                foreach (Transicion transicion in transicionesEpsilon)
                {
                    Estado destino = transicion.EstadoDestino;

                    if (resultado.Add(destino))
                    {
                        pendientes.Push(destino);
                    }
                }
            }

            return resultado;
        }

        private string FormatearEstados(IEnumerable<Estado> estados)
        {
            List<string> nombres = estados
                .Select(e => e.Nombre)
                .OrderBy(nombre => nombre, StringComparer.Ordinal)
                .ToList();

            return nombres.Count == 0
                ? "∅"
                : "{ " + string.Join(", ", nombres) + " }";
        }
    }
}
