using System;

namespace OpheliaSuiteV2.Core.DataAccess.MicroSQL.Exceptions {

    /// <summary>
    /// Encapsula los datos de la excepción que ocurre cuando se
    /// referencia una propiedad foránea que no existe en  la entidad
    /// referenciada
    /// </summary>
    public sealed class ForeignPropertyDontExistsException : Exception {

        #region Builders

        /// <summary>
        /// Inicializa una nueva instancia de la clase
        /// </summary>
        /// <param name="message">Mensaje de la excepción</param>
        public ForeignPropertyDontExistsException(string message) : base(message) {
        }

        #endregion
    }
}
