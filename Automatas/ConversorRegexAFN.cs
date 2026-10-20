using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


using System;
using SimuladorAutomatas.Modelos;

namespace SimuladorAutomatas.Automatas
{
    /// <summary>
    /// Convierte expresiones regulares en un AFN utilizando
    /// el algoritmo de Thompson.
    ///
    /// Operadores admitidos:
    /// |  alternativa
    /// *  cero o más repeticiones
    /// +  una o más repeticiones
    /// ?  cero o una repetición
    /// () agrupación
    ///
    /// Los símbolos del alfabeto son caracteres individuales.
    /// Se pueden escapar los operadores con una barra invertida.
    /// </summary>
    public class ConversorRegexAFN
    {
        private class Fragmento
        {
            public Estado Inicial { get; set; }
            public Estado Final { get; set; }

            public Fragmento(Estado inicial, Estado final)
            {
                Inicial = inicial;
                Final = final;
            }
        }

        private AFN afn;
        private string expresion;
        private int posicion;
        private int contadorEstados;

        /// <summary>
        /// Convierte una expresión regular en un AFN.
        /// </summary>
        public AFN Convertir(string expresionRegular)
        {
            if (expresionRegular == null)
            {
                throw new ArgumentNullException(
                    nameof(expresionRegular),
                    "Debe ingresar una expresión regular.");
            }

            expresion = expresionRegular;
            posicion = 0;
            contadorEstados = 0;
            afn = new AFN();

            Fragmento resultado = ParsearAlternativa();

            if (posicion < expresion.Length)
            {
                throw new FormatException(
                    "Símbolo inesperado en la posición "
                    + posicion + ": '" + expresion[posicion] + "'.");
            }

            afn.EstablecerEstadoInicial(resultado.Inicial);
            afn.AgregarEstadoFinal(resultado.Final);

            return afn;
        }

        // --------------------------------------------------
        // PARSER: ALTERNATIVA
        // Ejemplo: a|b
        // --------------------------------------------------
        private Fragmento ParsearAlternativa()
        {
            Fragmento izquierda = ParsearConcatenacion();

            while (Coincide('|'))
            {
                Avanzar();

                Fragmento derecha = ParsearConcatenacion();

                Estado nuevoInicial = CrearEstado();
                Estado nuevoFinal = CrearEstado();

                AgregarEpsilon(nuevoInicial, izquierda.Inicial);
                AgregarEpsilon(nuevoInicial, derecha.Inicial);

                AgregarEpsilon(izquierda.Final, nuevoFinal);
                AgregarEpsilon(derecha.Final, nuevoFinal);

                izquierda = new Fragmento(
                    nuevoInicial,
                    nuevoFinal);
            }

            return izquierda;
        }

        // --------------------------------------------------
        // PARSER: CONCATENACIÓN
        // Ejemplo: abc
        // --------------------------------------------------
        private Fragmento ParsearConcatenacion()
        {
            Fragmento resultado = null;

            while (HaySimbolo() &&
                   !Coincide('|') &&
                   !Coincide(')'))
            {
                Fragmento siguiente = ParsearRepeticion();

                if (resultado == null)
                {
                    resultado = siguiente;
                }
                else
                {
                    AgregarEpsilon(
                        resultado.Final,
                        siguiente.Inicial);

                    resultado = new Fragmento(
                        resultado.Inicial,
                        siguiente.Final);
                }
            }

            // Permite expresiones vacías dentro de alternativas,
            // por ejemplo a|, (a|) o una expresión vacía.
            if (resultado == null)
            {
                Estado inicialVacio = CrearEstado();
                Estado finalVacio = CrearEstado();

                AgregarEpsilon(inicialVacio, finalVacio);

                resultado = new Fragmento(
                    inicialVacio,
                    finalVacio);
            }

            return resultado;
        }

        // --------------------------------------------------
        // PARSER: REPETICIONES
        // Ejemplos: a*, a+, a?
        // --------------------------------------------------
        private Fragmento ParsearRepeticion()
        {
            Fragmento resultado = ParsearPrimario();

            while (HaySimbolo())
            {
                if (Coincide('*'))
                {
                    Avanzar();

                    Estado nuevoInicial = CrearEstado();
                    Estado nuevoFinal = CrearEstado();

                    AgregarEpsilon(nuevoInicial, resultado.Inicial);
                    AgregarEpsilon(nuevoInicial, nuevoFinal);

                    AgregarEpsilon(resultado.Final, resultado.Inicial);
                    AgregarEpsilon(resultado.Final, nuevoFinal);

                    resultado = new Fragmento(
                        nuevoInicial,
                        nuevoFinal);
                }
                else if (Coincide('+'))
                {
                    Avanzar();

                    Estado nuevoInicial = CrearEstado();
                    Estado nuevoFinal = CrearEstado();

                    AgregarEpsilon(nuevoInicial, resultado.Inicial);

                    AgregarEpsilon(resultado.Final, resultado.Inicial);
                    AgregarEpsilon(resultado.Final, nuevoFinal);

                    resultado = new Fragmento(
                        nuevoInicial,
                        nuevoFinal);
                }
                else if (Coincide('?'))
                {
                    Avanzar();

                    Estado nuevoInicial = CrearEstado();
                    Estado nuevoFinal = CrearEstado();

                    AgregarEpsilon(nuevoInicial, resultado.Inicial);
                    AgregarEpsilon(nuevoInicial, nuevoFinal);

                    AgregarEpsilon(resultado.Final, nuevoFinal);

                    resultado = new Fragmento(
                        nuevoInicial,
                        nuevoFinal);
                }
                else
                {
                    break;
                }
            }

            return resultado;
        }

        // --------------------------------------------------
        // PARSER: SÍMBOLO O GRUPO
        // Ejemplos: a, (ab), \*
        // --------------------------------------------------
        private Fragmento ParsearPrimario()
        {
            if (!HaySimbolo())
            {
                throw new FormatException(
                    "La expresión está incompleta.");
            }

            if (Coincide('('))
            {
                Avanzar();

                Fragmento interior = ParsearAlternativa();

                if (!Coincide(')'))
                {
                    throw new FormatException(
                        "Falta cerrar un paréntesis.");
                }

                Avanzar();
                return interior;
            }

            if (Coincide(')') ||
                Coincide('|') ||
                Coincide('*') ||
                Coincide('+') ||
                Coincide('?'))
            {
                throw new FormatException(
                    "Operador inesperado en la posición "
                    + posicion + ".");
            }

            char simbolo;

            if (Coincide('\\'))
            {
                Avanzar();

                if (!HaySimbolo())
                {
                    throw new FormatException(
                        "La barra invertida debe ir seguida "
                        + "de un símbolo.");
                }

                simbolo = expresion[posicion];
                Avanzar();
            }
            else
            {
                simbolo = expresion[posicion];
                Avanzar();
            }

            Estado inicial = CrearEstado();
            Estado final = CrearEstado();

            afn.AgregarTransicionAFN(
                inicial,
                simbolo.ToString(),
                final);

            return new Fragmento(inicial, final);
        }

        // --------------------------------------------------
        // MÉTODOS AUXILIARES
        // --------------------------------------------------
        private Estado CrearEstado()
        {
            Estado estado = new Estado(
                "q" + contadorEstados);

            contadorEstados++;

            afn.AgregarEstado(estado);

            return estado;
        }

        private void AgregarEpsilon(
            Estado origen,
            Estado destino)
        {
            afn.AgregarTransicionAFN(
                origen,
                "ε",
                destino);
        }

        private bool HaySimbolo()
        {
            return posicion < expresion.Length;
        }

        private bool Coincide(char simbolo)
        {
            return HaySimbolo() &&
                   expresion[posicion] == simbolo;
        }

        private void Avanzar()
        {
            posicion++;
        }
    }
}
