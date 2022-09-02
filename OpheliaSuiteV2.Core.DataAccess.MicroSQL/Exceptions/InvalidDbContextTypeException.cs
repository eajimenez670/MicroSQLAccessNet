using System;

namespace OpheliaSuiteV2.Core.DataAccess.MicroSQL.Exceptions {
    /// <summary>
    /// Encapsula los datos de la excepción que ocurre
    /// cuando se configura un tipo de contexto inválido
    /// </summary>
    public class InvalidDbContextTypeException : Exception {

        #region Builders

        /// <summary>
        /// Inicializa una nueva instancia de la clase
        /// </summary>
        /// <param name="message">Mensaje de la excepción</param>
        public InvalidDbContextTypeException(string message) : base(message) { }

        #endregion
    }
}
