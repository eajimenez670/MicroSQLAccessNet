using OpheliaSuiteV2.Core.DataAccess.MicroSQL.ORM;

using System;

namespace OpheliaSuiteV2.Core.DataAccess.MicroSQL.Exceptions {

    /// <summary>
    /// Encapsula los datos de la excepción que ocurre
    /// cuando se intenta agregar una entidad que ya existe 
    /// con estado <see cref="EntityState.Unchanged"/> en el contexto
    /// </summary>
    public sealed class EntityAlreadyExistsException : Exception {

        #region Builders

        /// <summary>
        /// Inicializa una nueva instancia de la clase
        /// </summary>
        /// <param name="message">Mensaje de la excepción</param>
        public EntityAlreadyExistsException(string message) : base(message) {
        }

        #endregion
    }
}
