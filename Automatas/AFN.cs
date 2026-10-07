using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SimuladorAutomatas.Modelos;

namespace SimuladorAutomatas.Automatas
{
    public class AFN : Automata
    {
        public string NombreTipo
        {
            get
            {
                return "Autómata Finito No Determinista";
            }
        }

        public void AgregarTransicionAFN(
            Estado origen,
            string simbolo,
            Estado destino)
        {
            AgregarTransicion(origen, simbolo, destino);
        }
    }
}