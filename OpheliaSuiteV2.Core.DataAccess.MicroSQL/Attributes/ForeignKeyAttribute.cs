using System;

namespace OpheliaSuiteV2.Core.DataAccess.MicroSQL.Attributes {

    /// <summary>
    /// Marca las propiedades que son foráneas a otras entidades
    /// creando así la relación entre ellas
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
    public sealed class ForeignKeyAttribute : Attribute {

        #region Properties

        /// <summary>
        /// Nombre de la propiedad de navegación
        /// </summary>
        public string NavigationProperty { get; private set; }
        /// <summary>
        /// Nombre de la propiedad en la entidad foránea
        /// </summary>
        public string ForeignProperty { get; set; }

        #endregion

        #region Builders

        /// <summary>
        /// Inicializa una nueva instancia de la clase
        /// </summary>
        /// <param name="navigationProperty">Nombre de la propiedad de navegación</param>
        /// <param name="foreignProperty">Nombre de la propiedad en la entidad foránea</param>
        public ForeignKeyAttribute(string navigationProperty, string foreignProperty = null) {
            NavigationProperty = (string.IsNullOrWhiteSpace(navigationProperty) ? null : navigationProperty.Trim());
            if (NavigationProperty == null)
                throw Error.ArgumentException(nameof(navigationProperty));
            ForeignProperty = (string.IsNullOrWhiteSpace(foreignProperty) ? null : foreignProperty.Trim());
        }

        #endregion
    }
}
