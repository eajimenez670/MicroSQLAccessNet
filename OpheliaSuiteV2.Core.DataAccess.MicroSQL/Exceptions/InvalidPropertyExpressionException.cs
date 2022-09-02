using System;

namespace OpheliaSuiteV2.Core.DataAccess.MicroSQL.Exceptions {

    /// <summary>
    /// Encapsula los datos de la excepción que ocurre cuando
    /// se proporciona una expresión de propiedad inválida
    /// </summary>
    public sealed class InvalidPropertyExpressionException : Exception {

        #region Builders

        /// <summary>
        /// Inicializa una nueva instancia de la clase
        /// </summary>
        /// <param name="message">Mensaje de la excepción</param>
        public InvalidPropertyExpressionException(string message) : base(message) {
        }


        #endregion
    }
}
