using System;

namespace OpheliaSuiteV2.Core.DataAccess.MicroSQL.Exceptions {

    /// <summary>
    /// Encapsula los datos de la excepción que ocurre cuando
    /// se intenta usar un repositorio que ún no ha sido inicializado
    /// </summary>
    public sealed class NotInitializedRepositoryException : Exception {

        #region Builders

        /// <summary>
        /// Inicializa una nueva instancia de la clase
        /// </summary>
        /// <param name="message">Mensaje de la excepción</param>
        public NotInitializedRepositoryException(string message) : base(message) {
        }

        #endregion
    }
}
