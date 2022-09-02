using OpheliaSuiteV2.Core.DataAccess.MicroSQL.Attributes;

using System;

namespace OpheliaSuiteV2.Core.DataAccess.MicroSQL.Exceptions {

    /// <summary>
    /// Encapsula los datos de la excepción que ocurre
    /// cuando el tipo de dato de la propiedad marcada
    /// como <see cref="KeyAttribute"/> no se ajusta al
    /// tipo de llave <see cref="KeyType"/> configurada
    /// </summary>
    public sealed class InvalidTypeEntityKeyException : Exception {

        #region Builders

        /// <summary>
        /// Inicializa una nueva instancia de la clase
        /// </summary>
        /// <param name="message">Mensaje de la excepción</param>
        public InvalidTypeEntityKeyException(string message) : base(message) {
        }

        #endregion
    }
}
