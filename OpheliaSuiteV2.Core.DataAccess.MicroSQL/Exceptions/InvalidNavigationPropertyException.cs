using System;

namespace OpheliaSuiteV2.Core.DataAccess.MicroSQL.Exceptions {

    /// <summary>
    /// Encapsula los datos de la excepción que ocurre cuando
    /// se asigna una propiedad de navegación que cuyo tipo
    /// de dato no es válido
    /// </summary>
    public sealed class InvalidNavigationPropertyException : Exception {

        #region Builders

        /// <summary>
        /// Inicializa una nueva instancia de la clase
        /// </summary>
        /// <param name="message">Mensaje de la excepción</param>
        public InvalidNavigationPropertyException(string message) : base(message) {
        }

        #endregion
    }
}
