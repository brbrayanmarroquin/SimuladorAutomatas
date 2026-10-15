using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System;
using System.Text.RegularExpressions;

namespace SimuladorAutomatas.Servicios
{
    /// <summary>
    /// Contiene los métodos necesarios para comprobar expresiones
    /// regulares y validar cadenas de texto.
    /// </summary>
    public class ValidadorExpresiones
    {
        /// <summary>
        /// Comprueba si una expresión regular tiene una sintaxis válida.
        /// Devuelve true si puede compilarse y false si contiene un error.
        /// </summary>
        /// <param name="expresion">Expresión regular que se desea comprobar.</param>
        /// <param name="mensajeError">Descripción del error, si existe.</param>
        public bool EsExpresionValida(
            string expresion,
            out string mensajeError)
        {
            mensajeError = string.Empty;

            // No se puede compilar una expresión que no ha sido proporcionada.
            if (expresion == null)
            {
                mensajeError = "Debe ingresar una expresión regular.";
                return false;
            }

            try
            {
                // Intentamos construir la expresión para comprobar su sintaxis.
                // El límite de tiempo evita que una expresión compleja tarde
                // demasiado en ejecutarse.
                Regex regex = new Regex(
                    expresion,
                    RegexOptions.CultureInvariant,
                    TimeSpan.FromSeconds(2));

                mensajeError = "La expresión regular es válida.";
                return true;
            }
            catch (ArgumentException ex)
            {
                // .NET lanza ArgumentException cuando el patrón es incorrecto.
                mensajeError = "La expresión regular contiene un error: "
                    + ex.Message;

                return false;
            }
        }

        /// <summary>
        /// Comprueba si una cadena completa coincide con la expresión regular.
        /// No basta con que solo una parte de la cadena coincida.
        /// </summary>
        /// <param name="expresion">Patrón que debe cumplir la cadena.</param>
        /// <param name="cadena">Texto que se desea validar.</param>
        /// <param name="mensajeError">Descripción de cualquier error.</param>
        public bool ValidarCadena(
            string expresion,
            string cadena,
            out string mensajeError)
        {
            mensajeError = string.Empty;

            // Primero comprobamos que la expresión regular sea correcta.
            if (!EsExpresionValida(expresion, out mensajeError))
            {
                return false;
            }

            // Una cadena vacía es válida como entrada: su aceptación
            // dependerá de la expresión regular que se haya escrito.
            if (cadena == null)
            {
                mensajeError = "Debe ingresar una cadena para validar.";
                return false;
            }

            try
            {
                // \A indica el inicio absoluto y \z el final absoluto.
                // Así exigimos que coincida la cadena completa.
                string patronCompleto = @"\A(?:" + expresion + @")\z";

                Regex regex = new Regex(
                    patronCompleto,
                    RegexOptions.CultureInvariant,
                    TimeSpan.FromSeconds(2));

                bool coincide = regex.IsMatch(cadena);

                mensajeError = coincide
                    ? "La cadena pertenece al lenguaje de la expresión."
                    : "La cadena no pertenece al lenguaje de la expresión.";

                return coincide;
            }
            catch (ArgumentException ex)
            {
                // Informamos de cualquier error que ocurra al evaluar el patrón.
                mensajeError = "No fue posible validar la cadena: "
                    + ex.Message;

                return false;
            }
            catch (RegexMatchTimeoutException)
            {
                // Informamos si la evaluación excede el tiempo permitido.
                mensajeError =
                    "La validación tardó demasiado. Revise la expresión.";

                return false;
            }
        }
    }
}