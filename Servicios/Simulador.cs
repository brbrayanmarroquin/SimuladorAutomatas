using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Collections.Generic;
using System.Linq;
using SimuladorAutomatas.Modelos;

namespace SimuladorAutomatas.Servicios
{
    public class Simulador
    {
        public bool SimularAFN(Automata automata, string cadena)
        {
            // Comenzamos con el estado inicial
            HashSet<Estado> estadosActuales = new HashSet<Estado>();

            estadosActuales.Add(automata.EstadoInicial);

            // Antes de leer la cadena,
            // buscamos todos los estados alcanzables mediante ε
            estadosActuales = CerraduraEpsilon(automata, estadosActuales);

            // Procesamos cada símbolo de la cadena
            foreach (char caracter in cadena)
            {
                string simbolo = caracter.ToString();

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

                // Después de consumir el símbolo,
                // volvemos a buscar las ε-transiciones.
                estadosActuales = CerraduraEpsilon(automata, nuevosEstados);

                // Si no quedan estados posibles,
                // la cadena es rechazada.
                if (estadosActuales.Count == 0)
                {
                    return false;
                }
            }

            // Revisamos si alguno de los estados actuales
            // es un estado final.
            foreach (Estado estado in estadosActuales)
            {
                if (estado.EsFinal)
                {
                    return true;
                }
            }

            return false;
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

                    if (!resultado.Contains(destino))
                    {
                        resultado.Add(destino);
                        pendientes.Push(destino);
                    }
                }
            }

            return resultado;
        }
    }
}