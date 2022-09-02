using System;

namespace OpheliaSuiteV2.Core.DataAccess.MicroSQL.Exceptions {

    /// <summary>
    /// Encapsula los datos de la excepción que ocurre cuando
    /// se proporciona una expresión no soportada en una consulta
    /// </summary>
    public sealed class UnsupportedExpressionException : Exception {

        #region Builders

        /// <summary>
        /// Inicializa una nueva instancia de la clase
        /// </summary>
        /// <param name="message">Mensaje de la excepción</param>
        public UnsupportedExpressionException(string message) : base(message) {
        }

        #endregion
    }
}
