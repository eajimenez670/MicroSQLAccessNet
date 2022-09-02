using System;

namespace OpheliaSuiteV2.Core.DataAccess.MicroSQL.Attributes {

    /// <summary>
    /// Marca la propiedad con el nombre de la columna a la que apunta
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public sealed class ColumnNameAttribute : Attribute {

        #region Properties

        /// <summary>
        /// Nombre de la columna
        /// </summary>
        public string Name { get; private set; }

        #endregion

        #region Builders

        /// <summary>
        /// Inicializa una nueva instancia de la clase
        /// </summary>
        /// <param name="name">Nombre de la columna</param>
        public ColumnNameAttribute(string name) {
            if (string.IsNullOrWhiteSpace(name))
                throw Error.ArgumentException(nameof(name));

            Name = name.Trim();
        }

        #endregion
    }
}
