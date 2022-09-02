using System;

namespace OpheliaSuiteV2.Core.DataAccess.MicroSQL.Exceptions {

    /// <summary>
    /// Encapsula los datos de la excepción que ocurre cuando
    /// se asigna una propiedad foránea cuyo tipo
    /// de dato no es válido o no corresponde con la propiedad local
    /// </summary>
    public sealed class InvalidForeignPropertyException : Exception {

        #region Builders

        /// <summary>
        /// Inicializa una nueva instancia de la clase
        /// </summary>
        /// <param name="message">Mensaje de la excepción</param>
        public InvalidForeignPropertyException(string message) : base(message) {
        }

        #endregion
    }
}
