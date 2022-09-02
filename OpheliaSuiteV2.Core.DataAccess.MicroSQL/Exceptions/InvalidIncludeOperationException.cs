using System;

namespace OpheliaSuiteV2.Core.DataAccess.MicroSQL.Exceptions {

    /// <summary>
    /// Encapsula los datos de la excepción que ocurre cuando se
    /// intenta realizar una inclusión en una consulta a un nivel
    /// no permitido
    /// </summary>
    public sealed class InvalidIncludeOperationException : Exception {

        #region Builders

        /// <summary>
        /// Inicializa una nueva instancia de la clase
        /// </summary>
        /// <param name="message">Mensaje de la excepción</param>
        public InvalidIncludeOperationException(string message) : base(message) {
        }

        #endregion
    }
}
