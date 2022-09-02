using System;

namespace OpheliaSuiteV2.Core.DataAccess.MicroSQL.Exceptions {

    /// <summary>
    /// Encapsula los datos de la excepción que ocurre cuando se
    /// referencia una propiedad de navegación que no existe en  la entidad
    /// </summary>
    public sealed class NavigationPropertyDontExistsException : Exception {

        #region Builders

        /// <summary>
        /// Inicializa una nueva instancia de la clase
        /// </summary>
        /// <param name="message">Mensaje de la excepción</param>
        public NavigationPropertyDontExistsException(string message) : base(message) {
        }

        #endregion
    }
}
