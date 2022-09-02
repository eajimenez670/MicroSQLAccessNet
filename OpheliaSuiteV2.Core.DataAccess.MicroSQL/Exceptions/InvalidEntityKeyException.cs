using OpheliaSuiteV2.Core.DataAccess.MicroSQL.Attributes;
using System;

namespace OpheliaSuiteV2.Core.DataAccess.MicroSQL.Exceptions {

    /// <summary>
    /// Encapsula los datos de la excepción que ocurre
    /// cuando se define una entidad con más de una llave
    /// primaria <see cref="KeyAttribute"/>
    /// </summary>
    public sealed class InvalidEntityKeyException : Exception {

        #region Builders

        /// <summary>
        /// Inicializa una nueva instancia de la clase
        /// </summary>
        /// <param name="message">Mensaje de la excepción</param>
        public InvalidEntityKeyException(string message) : base(message) {
        }

        #endregion
    }
}
