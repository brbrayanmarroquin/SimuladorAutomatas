using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Collections.Generic;
using System.Linq;
using SimuladorAutomatas.Modelos;

namespace SimuladorAutomatas.Automatas
{
    public class ConversorAFNaAFD
    {
        public AFD Convertir(AFN afn)
        {
            AFD afd = new AFD();

            // Diccionario para guardar los estados del AFD
            Dictionary<string, Estado> estadosAFD =
                new Dictionary<string, Estado>();

            // Cola de conjuntos de estados pendientes
            Queue<HashSet<Estado>> pendientes =
                new Queue<HashSet<Estado>>();

            // Obtenemos la cerradura epsilon del estado inicial
            HashSet<Estado> conjuntoInicial =
                ObtenerCerraduraEpsilon(
                    afn,
                    new HashSet<Estado> { afn.EstadoInicial });

            // Creamos el nombre del estado inicial
            string nombreInicial =
                CrearNombreEstado(conjuntoInicial);

            // El estado será final si contiene algún estado final del AFN
            bool esFinal =
                conjuntoInicial.Any(e => e.EsFinal);

            Estado estadoInicial =
                new Estado(nombreInicial, true, esFinal);

            // Guardamos el estado
            estadosAFD[nombreInicial] = estadoInicial;

            // Lo agregamos al AFD
            afd.AgregarEstado(estadoInicial);
            afd.EstablecerEstadoInicial(estadoInicial);

            if (esFinal)
            {
                afd.AgregarEstadoFinal(estadoInicial);
            }

            // Agregamos el conjunto inicial a la cola
            pendientes.Enqueue(conjuntoInicial);

            // Procesamos todos los conjuntos pendientes
            while (pendientes.Count > 0)
            {
                HashSet<Estado> conjuntoActual =
                    pendientes.Dequeue();

                string nombreActual =
                    CrearNombreEstado(conjuntoActual);

                Estado estadoActual =
                    estadosAFD[nombreActual];

                // Revisamos cada símbolo del alfabeto
                foreach (string simbolo in afn.Alfabeto)
                {
                    // Movimiento con el símbolo
                    HashSet<Estado> movimiento =
                        ObtenerMovimiento(
                            afn,
                            conjuntoActual,
                            simbolo);

                    // Cerradura epsilon del resultado
                    HashSet<Estado> destino =
                        ObtenerCerraduraEpsilon(
                            afn,
                            movimiento);

                    // Si no hay destino, continuamos
                    if (destino.Count == 0)
                    {
                        continue;
                    }

                    // Nombre del conjunto destino
                    string nombreDestino =
                        CrearNombreEstado(destino);

                    // Si todavía no existe, lo creamos
                    if (!estadosAFD.ContainsKey(nombreDestino))
                    {
                        bool destinoEsFinal =
                            destino.Any(e => e.EsFinal);

                        Estado nuevoEstado =
                            new Estado(
                                nombreDestino,
                                false,
                                destinoEsFinal);

                        estadosAFD[nombreDestino] =
                            nuevoEstado;

                        afd.AgregarEstado(nuevoEstado);

                        if (destinoEsFinal)
                        {
                            afd.AgregarEstadoFinal(nuevoEstado);
                        }

                        // Lo procesaremos después
                        pendientes.Enqueue(destino);
                    }

                    // Obtenemos el estado destino
                    Estado estadoDestino =
                        estadosAFD[nombreDestino];

                    // Agregamos la transición
                    afd.AgregarTransicionAFD(
                        estadoActual,
                        simbolo,
                        estadoDestino);
                }
            }

            return afd;
        }


        // Obtiene los estados alcanzables usando un símbolo
        private HashSet<Estado> ObtenerMovimiento(
            AFN afn,
            HashSet<Estado> estados,
            string simbolo)
        {
            HashSet<Estado> destinos =
                new HashSet<Estado>();

            foreach (Estado estado in estados)
            {
                var transiciones =
                    afn.Transiciones
                        .Where(t =>
                            t.EstadoOrigen == estado &&
                            t.Simbolo == simbolo);

                foreach (Transicion transicion in transiciones)
                {
                    destinos.Add(
                        transicion.EstadoDestino);
                }
            }

            return destinos;
        }


        // Obtiene todos los estados alcanzables mediante epsilon
        private HashSet<Estado> ObtenerCerraduraEpsilon(
            AFN afn,
            HashSet<Estado> estadosIniciales)
        {
            HashSet<Estado> resultado =
                new HashSet<Estado>(estadosIniciales);

            Stack<Estado> pendientes =
                new Stack<Estado>(estadosIniciales);

            while (pendientes.Count > 0)
            {
                Estado estadoActual =
                    pendientes.Pop();

                var transicionesEpsilon =
                    afn.Transiciones
                        .Where(t =>
                            t.EstadoOrigen == estadoActual &&
                            t.Simbolo == "ε");

                foreach (Transicion transicion
                    in transicionesEpsilon)
                {
                    Estado destino =
                        transicion.EstadoDestino;

                    if (!resultado.Contains(destino))
                    {
                        resultado.Add(destino);
                        pendientes.Push(destino);
                    }
                }
            }

            return resultado;
        }


        // Crea un nombre único para cada conjunto de estados
        private string CrearNombreEstado(
            HashSet<Estado> estados)
        {
            return string.Join(
                "_",
                estados
                    .OrderBy(e => e.Nombre)
                    .Select(e => e.Nombre));
        }
    }
}