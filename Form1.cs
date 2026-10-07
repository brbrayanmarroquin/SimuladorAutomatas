using SimuladorAutomatas.Automatas;
using SimuladorAutomatas.Modelos;
using SimuladorAutomatas.Servicios;
using System;
using System.Windows.Forms;
using SimuladorAutomatas.Automatas;
namespace SimuladorAutomatas
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            ProbarEpsilon();



        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
        private void ProbarEpsilon()
        {
            AFN automata = new AFN();

            // Creamos los estados
            Estado q0 = new Estado("q0");
            Estado q1 = new Estado("q1");
            Estado q2 = new Estado("q2");
            Estado q3 = new Estado("q3");

            // Agregamos los estados
            automata.AgregarEstado(q0);
            automata.AgregarEstado(q1);
            automata.AgregarEstado(q2);
            automata.AgregarEstado(q3);

            // Estado inicial
            automata.EstablecerEstadoInicial(q0);

            // Estado final
            automata.AgregarEstadoFinal(q3);

            // Primer camino:
            // q0 --a--> q1 --b--> q3
            automata.AgregarTransicionAFN(q0, "a", q1);
            automata.AgregarTransicionAFN(q1, "b", q3);

            // Segundo camino:
            // q0 --ε--> q2 --a--> q3
            automata.AgregarTransicionAFN(q0, "ε", q2);
            automata.AgregarTransicionAFN(q2, "a", q3);

            // Creamos el simulador
            Simulador simulador = new Simulador();

            // Probamos diferentes cadenas
            bool resultadoA = simulador.SimularAFN(automata, "a");
            bool resultadoAB = simulador.SimularAFN(automata, "ab");
            bool resultadoB = simulador.SimularAFN(automata, "b");
            bool resultadoAA = simulador.SimularAFN(automata, "aa");

            MessageBox.Show(
                "Prueba de AFN con varios caminos:\n\n" +
                "Cadena 'a': " +
                (resultadoA ? "ACEPTADA" : "RECHAZADA") + "\n" +
                "Cadena 'ab': " +
                (resultadoAB ? "ACEPTADA" : "RECHAZADA") + "\n" +
                "Cadena 'b': " +
                (resultadoB ? "ACEPTADA" : "RECHAZADA") + "\n" +
                "Cadena 'aa': " +
                (resultadoAA ? "ACEPTADA" : "RECHAZADA"),
                "Prueba completa del AFN"
            );
        }

    }
}
