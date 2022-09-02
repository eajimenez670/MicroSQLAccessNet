using System;

namespace OpheliaSuiteV2.Core.DataAccess.MicroSQL.Exceptions {
    /// <summary>
    /// Encapsula los datos de la excepción que ocurre
    /// cuando se usa un parámetro que no ha sifo definido
    /// dentro de un comando Sql
    /// </summary>
    public class UndefinedParameterException : Exception {

        #region Builders

        /// <summary>
        /// Inicializa una nueva instancia de la clase
        /// </summary>
        /// <param name="message">Mensaje de la excepción</param>
        public UndefinedParameterException(string message) : base(message) { }

        #endregion
    }
}
