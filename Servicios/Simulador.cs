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
            // Comenzamos desde el estado inicial
            HashSet<Estado> estadosActuales = new HashSet<Estado>();

            estadosActuales.Add(automata.EstadoInicial);

            // Procesamos cada símbolo de la cadena
            foreach (char caracter in cadena)
            {
                string simbolo = caracter.ToString();

                HashSet<Estado> nuevosEstados = new HashSet<Estado>();

                foreach (Estado estadoActual in estadosActuales)
                {
                    // Buscamos las transiciones que coincidan
                    // con el estado y el símbolo actual
                    List<Transicion> transiciones = automata.Transiciones
                        .Where(t => t.EstadoOrigen == estadoActual &&
                                    t.Simbolo == simbolo)
                        .ToList();

                    foreach (Transicion transicion in transiciones)
                    {
                        nuevosEstados.Add(transicion.EstadoDestino);
                    }
                }

                estadosActuales = nuevosEstados;

                // Si ya no tenemos estados posibles,
                // la cadena no puede ser aceptada.
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
    }
}