using System;

namespace OpheliaSuiteV2.Core.DataAccess.MicroSQL.Attributes {

    /// <summary>
    /// Marca la entidad con el nombre de la tabla a la que
    /// apunta
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public sealed class TableNameAttribute : Attribute {

        #region Properties

        /// <summary>
        /// Nombre de la tabla
        /// </summary>
        public string Name {
            get; private set;
        }

        #endregion

        #region Builders

        /// <summary>
        /// Inicializa una nueva instancia de la clase
        /// </summary>
        /// <param name="name">Nombre de la tabla</param>
        public TableNameAttribute(string name) {
            if (string.IsNullOrWhiteSpace(name))
                throw Error.ArgumentException(nameof(name));

            Name = name.Trim();
        }

        #endregion
    }
}
