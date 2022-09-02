using System;

namespace OpheliaSuiteV2.Core.DataAccess.MicroSQL.Exceptions {

    /// <summary>
    /// Encapsula los datos de la excepción que ocurre cuando
    /// se asigna un valor nulo a un parámetro. Se debe usar el 
    /// tipo <see cref="Nullable{T}"/>
    /// </summary>
    public sealed class InvalidNullableParameterException : Exception {

        #region Builders

        /// <summary>
        /// Inicializa una nueva instancia de la clase
        /// </summary>
        /// <param name="message">Mensaje de la excepción</param>
        public InvalidNullableParameterException(string message) : base(message) {
        }

        #endregion
    }
}
