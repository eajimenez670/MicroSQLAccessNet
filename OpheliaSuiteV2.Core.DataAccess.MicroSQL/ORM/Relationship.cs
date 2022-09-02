using System;
using System.Collections.Generic;
using System.Reflection;

namespace OpheliaSuiteV2.Core.DataAccess.MicroSQL.ORM {

    /// <summary>
    /// Encapsula los datos de la relación
    /// entre dos entidades
    /// </summary>
    public sealed class Relationship {

        #region Properties

        /// <summary>
        /// Entidad principal o cabecera
        /// </summary>
        public Type PrincipalEntity { get; private set; }

        /// <summary>
        /// Entidad foránea o detalle
        /// </summary>
        public Type ForeignEntity { get; private set; }

        /// <summary>
        /// Propiedad de navegación en la entidad foránea
        /// </summary>
        public PropertyInfo NavigationProperty { get; private set; }

        /// <summary>
        /// Propiedades relacionadas
        /// </summary>
        public List<PropertyPair> Properties { get; private set; }

        #endregion

        #region Builders

        /// <summary>
        /// Inicializa una nueva instancia de la clase
        /// </summary>
        /// <param name="principalEntity">Entidad principal o cabecera</param>
        /// <param name="foreignEntity">Entidad foránea o detalle</param>
        /// <param name="navigationProperty">Propiedad de navegación en la entidad foránea</param>
        /// <param name="properties">Propiedades relacionadas</param>
        public Relationship(Type principalEntity, Type foreignEntity, PropertyInfo navigationProperty, List<PropertyPair> properties) {
            PrincipalEntity = principalEntity ?? throw Error.ArgumentException(nameof(principalEntity));
            ForeignEntity = foreignEntity ?? throw Error.ArgumentException(nameof(foreignEntity));
            NavigationProperty = navigationProperty ?? throw Error.ArgumentException(nameof(foreignEntity));
            Properties = properties ?? throw Error.ArgumentException(nameof(properties));
        }

        #endregion
    }

    /// <summary>
    /// Encapsula las propiedades relacionadas entre dos entidades
    /// por medio de su clave
    /// </summary>
    public sealed class PropertyPair {

        #region Properties

        /// <summary>
        /// Propiedad en entidad principal
        /// </summary>
        public PropertyInfo PrincipalProperty { get; private set; }

        /// <summary>
        /// Propiedad en entidad foránea
        /// </summary>
        public PropertyInfo ForeignProperty { get; private set; }

        #endregion

        #region Builders

        /// <summary>
        /// Inicializa una nueva instancia de la clase
        /// </summary>
        /// <param name="principalProperty">Propiedad en entidad principal</param>
        /// <param name="foreignProperty">Propiedad en entidad foránea</param>
        public PropertyPair(PropertyInfo principalProperty, PropertyInfo foreignProperty) {
            PrincipalProperty = principalProperty;
            ForeignProperty = foreignProperty;
        }

        #endregion
    }
}
