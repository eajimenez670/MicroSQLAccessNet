using System;

namespace OpheliaSuiteV2.Core.DataAccess.MicroSQL.Exceptions {

    /// <summary>
    /// Encapsula los datos de la excepción que ocurre
    /// cuando se intenta realizar una operación CRUD
    /// que no es coherénte con el estado actual de la entidad
    /// </summary>
    public sealed class InvalidOperationEntityEntryException : Exception {

        #region Builders

        /// <summary>
        /// Inicializa una nueva instancia de la clase
        /// </summary>
        /// <param name="message">Mensaje de la excepción</param>
        public InvalidOperationEntityEntryException(string message) : base(message) {
        }

        #endregion
    }
}
