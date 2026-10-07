using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using System.Linq;
using SimuladorAutomatas.Modelos;

namespace SimuladorAutomatas.Automatas
{
    public class AFD : Automata
    {
        public string NombreTipo
        {
            get
            {
                return "Autómata Finito Determinista";
            }
        }

        public bool AgregarTransicionAFD(
            Estado origen,
            string simbolo,
            Estado destino)
        {
            if (simbolo == "ε")
            {
                throw new ArgumentException(
                    "Un AFD no puede tener transiciones epsilon.");
            }

            if (Transiciones.Any(t =>
                t.EstadoOrigen == origen &&
                t.Simbolo == simbolo))
            {
                throw new InvalidOperationException(
                    "El AFD ya tiene una transición para ese estado y símbolo.");
            }

            AgregarTransicion(origen, simbolo, destino);

            return true;
        }
    }
}