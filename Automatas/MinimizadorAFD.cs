using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Collections.Generic;
using System.Linq;
using SimuladorAutomatas.Modelos;


using System;
using System.Collections.Generic;
using System.Linq;
using SimuladorAutomatas.Modelos;

namespace SimuladorAutomatas.Automatas
{
    public class MinimizadorAFD
    {
        public AFD Minimizar(AFD afd)
        {
            if (afd == null)
                throw new ArgumentNullException(nameof(afd));

            if (afd.EstadoInicial == null)
                throw new InvalidOperationException(
                    "El AFD debe tener un estado inicial.");

            // 1. Obtener solamente los estados accesibles
            HashSet<Estado> accesibles = new HashSet<Estado>();
            Queue<Estado> cola = new Queue<Estado>();

            cola.Enqueue(afd.EstadoInicial);
            accesibles.Add(afd.EstadoInicial);

            while (cola.Count > 0)
            {
                Estado actual = cola.Dequeue();

                foreach (Transicion t in afd.Transiciones)
                {
                    if (t.EstadoOrigen == actual &&
                        accesibles.Add(t.EstadoDestino))
                    {
                        cola.Enqueue(t.EstadoDestino);
                    }
                }
            }

            List<Estado> estados = accesibles
                .OrderBy(e => e.Nombre)
                .ToList();

            // 2. Separar estados finales y no finales
            List<List<Estado>> grupos = new List<List<Estado>>();

            var finales = estados.Where(e => e.EsFinal).ToList();
            var noFinales = estados.Where(e => !e.EsFinal).ToList();

            if (noFinales.Count > 0)
                grupos.Add(noFinales);

            if (finales.Count > 0)
                grupos.Add(finales);

            // 3. Refinar los grupos hasta que sean estables
            bool huboCambios = true;

            while (huboCambios)
            {
                huboCambios = false;

                Dictionary<Estado, int> grupoDe =
                    new Dictionary<Estado, int>();

                for (int i = 0; i < grupos.Count; i++)
                {
                    foreach (Estado estado in grupos[i])
                        grupoDe[estado] = i;
                }

                List<List<Estado>> nuevosGrupos =
                    new List<List<Estado>>();

                foreach (List<Estado> grupo in grupos)
                {
                    Dictionary<string, List<Estado>> particiones =
                        new Dictionary<string, List<Estado>>();

                    foreach (Estado estado in grupo)
                    {
                        List<string> partes = new List<string>();

                        foreach (string simbolo in afd.Alfabeto
                                     .OrderBy(s => s))
                        {
                            Transicion t = afd.Transiciones.FirstOrDefault(
                                x => x.EstadoOrigen == estado &&
                                     x.Simbolo == simbolo);

                            // -1 representa una transición inexistente
                            string destino = t == null
                                ? "-1"
                                : grupoDe[t.EstadoDestino].ToString();

                            partes.Add(destino);
                        }

                        string firma = string.Join("|", partes);

                        if (!particiones.ContainsKey(firma))
                            particiones[firma] = new List<Estado>();

                        particiones[firma].Add(estado);
                    }

                    nuevosGrupos.AddRange(particiones.Values);

                    if (particiones.Count > 1)
                        huboCambios = true;
                }

                grupos = nuevosGrupos;
            }

            // 4. Crear el AFD minimizado
            AFD resultado = new AFD();
            resultado.Alfabeto.UnionWith(afd.Alfabeto);

            Dictionary<Estado, Estado> equivalencias =
                new Dictionary<Estado, Estado>();

            foreach (List<Estado> grupo in grupos)
            {
                string nombre = string.Join(
                    "_",
                    grupo.Select(e => e.Nombre).OrderBy(n => n));

                bool esInicial = grupo.Contains(afd.EstadoInicial);
                bool esFinal = grupo.Any(e => e.EsFinal);

                Estado nuevo = new Estado(nombre, esInicial, esFinal);

                resultado.AgregarEstado(nuevo);

                if (esInicial)
                    resultado.EstablecerEstadoInicial(nuevo);

                if (esFinal)
                    resultado.AgregarEstadoFinal(nuevo);

                foreach (Estado original in grupo)
                    equivalencias[original] = nuevo;
            }

            // 5. Reconstruir las transiciones
            foreach (Transicion t in afd.Transiciones)
            {
                if (!equivalencias.ContainsKey(t.EstadoOrigen) ||
                    !equivalencias.ContainsKey(t.EstadoDestino))
                    continue;

                Estado origen = equivalencias[t.EstadoOrigen];
                Estado destino = equivalencias[t.EstadoDestino];

                // Evitar transiciones duplicadas
                bool existe = resultado.Transiciones.Any(x =>
                    x.EstadoOrigen == origen &&
                    x.Simbolo == t.Simbolo);

                if (!existe)
                {
                    resultado.AgregarTransicionAFD(
                        origen, t.Simbolo, destino);
                }
            }

            return resultado;
        }
    }
}
