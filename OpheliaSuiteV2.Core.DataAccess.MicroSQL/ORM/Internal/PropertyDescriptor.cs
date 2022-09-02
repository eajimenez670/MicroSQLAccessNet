using OpheliaSuiteV2.Core.DataAccess.MicroSQL.Attributes;

using System.Reflection;

namespace OpheliaSuiteV2.Core.DataAccess.MicroSQL.ORM.Internal {

    /// <summary>
    /// Encapsula la descripción de una propiedad
    /// </summary>
    internal sealed class PropertyDescriptor {

        #region Properties

        /// <summary>
        /// Información de la propiedad
        /// </summary>
        public PropertyInfo Property {
            get; private set;
        }

        /// <summary>
        /// Nombre de la propiedad
        /// </summary>
        public string Name {
            get; private set;
        }

        /// <summary>
        /// Nombre de la columna en la tabla
        /// </summary>
        public string ColumnName { get; private set; }

        /// <summary>
        /// Valor que indica si la propiedad compone
        /// la llave primaria de la entidad y es la principal
        /// marcada como <see cref="KeyAttribute"/>
        /// </summary>
        public bool IsKey {
            get; private set;
        }

        /// <summary>
        /// Valor que indica si la propiedad compone
        /// la llave primaria de la entidad y es auxiliar
        /// marcada como <see cref="KeyPartAttribute"/>
        /// </summary>
        public bool IsKeyPart {
            get; private set;
        }

        #endregion

        #region Builders

        /// <summary>
        /// Inicializa una nueva instancia de la clase
        /// </summary>
        /// <param name="property">Propiedad</param>
        public PropertyDescriptor(PropertyInfo property) {
            Property = property ?? throw Error.ArgumentException(nameof(property));
            Name = property.Name;
            ColumnName = Helpers.GetColumnName(property);
            SetIsKey();
            SetIsKeyPart();
        }

        #endregion

        #region Methods

        /// <summary>
        /// Asigna un valor que indica si la propiedad
        /// hace parte de la llave primaria de la entidad
        /// </summary>
        private void SetIsKey() {
            IsKey = (Property.GetCustomAttribute<KeyAttribute>() != null);
        }

        /// <summary>
        /// Asigna un valor que indica si la propiedad
        /// hace parte de la llave primaria de la entidad
        /// </summary>
        private void SetIsKeyPart() {
            IsKeyPart = (Property.GetCustomAttribute<KeyPartAttribute>() != null);
        }

        #endregion
    }
}
