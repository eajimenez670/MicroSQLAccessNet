using OpheliaSuiteV2.Core.DataAccess.MicroSQL.Attributes;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace OpheliaSuiteV2.Core.DataAccess.MicroSQL.ORM.Internal {

    /// <summary>
    /// Encapsula la descripción de una entidad
    /// </summary>
    internal sealed class EntityDescriptor {

        #region Properties

        /// <summary>
        /// Tipo de la entidad
        /// </summary>
        public Type EntityType { get; private set; }

        /// <summary>
        /// Nombre de la tabla
        /// </summary>
        public string TableName { get; private set; }

        /// <summary>
        /// Propiedades de la entidad mapeadas por su nombre de campo
        /// </summary>
        public Dictionary<string, PropertyDescriptor> Properties { get; private set; }

        /// <summary>
        /// Lista de relaciones
        /// </summary>
        public List<Relationship> Relationships { get; private set; } = new List<Relationship>();

        #endregion

        #region Builders

        /// <summary>
        /// Inicializa una nueva instancia de la clase
        /// </summary>
        /// <param name="entityType">Tipo de la entidad</param>
        public EntityDescriptor(Type entityType) {
            EntityType = entityType ?? throw Error.ArgumentException(nameof(entityType));
            EnsureEntityKey();
            TableName = Helpers.GetTableName(EntityType);
            Properties = new Dictionary<string, PropertyDescriptor>();
            Relationships = new List<Relationship>();
            GetProperties();
            GetRelationships();
        }

        #endregion

        #region Methods

        /// <summary>
        /// Obtiene el descriptor de la propiedad por su nombre de columna
        /// </summary>
        /// <param name="columnName">Nombre de la columna</param>
        /// <returns>Descriptor de la propiedad</returns>
        public PropertyDescriptor GetPropertyByColumnName(string columnName) {
            Properties.TryGetValue(columnName, out PropertyDescriptor descriptor);
            return descriptor;
        }

        /// <summary>
        /// Obtiene el descriptor de la propiedad por su nombre de la propiedad
        /// </summary>
        /// <param name="propertyName">Nombre de la propiedad</param>
        /// <returns>Descriptor de la propiedad</returns>
        public PropertyDescriptor GetPropertyByName(string propertyName) {
            if (Properties.Any(kv => kv.Value.Property.Name == propertyName))
                return Properties.First(kv => kv.Value.Property.Name == propertyName).Value;
            return null;
        }

        /// <summary>
        /// Asegura que la entidad tenga al menos
        /// una propiedad marcada como llave primaria
        /// </summary>
        private void EnsureEntityKey() {
            List<PropertyInfo> propKeys = EntityType.GetProperties().Where(p => p.GetCustomAttribute<KeyAttribute>() != null).ToList();
            if (propKeys.Count != 1)
                throw Error.InvalidEntityKeyException(EntityType);

            KeyAttribute keyAttribute = propKeys.First().GetCustomAttribute<KeyAttribute>();
            Type typePropKey = propKeys.First().PropertyType;
            //Validamos el tipo de dato de la propiedad llave
            switch (keyAttribute.Type) {
                case KeyType.Autoincrement:
                case KeyType.Identity:
                    if (!typePropKey.IsAssignableFrom(typeof(short)) && !typePropKey.IsAssignableFrom(typeof(int)) && !typePropKey.IsAssignableFrom(typeof(long))) {
                        throw Error.InvalidTypeEntityKeyException(EntityType.Name);
                    }
                    break;
                case KeyType.Uniqueidentifier:
                    if (!typePropKey.IsAssignableFrom(typeof(Guid)) && !typePropKey.IsAssignableFrom(typeof(string))) {
                        throw Error.InvalidTypeEntityKeyException(EntityType.Name);
                    }
                    break;
            }
        }

        /// <summary>
        /// Obtiene las propiedades
        /// </summary>
        private void GetProperties() {
            //Obtenemos solo las propiedades válidas y que no han sido ignoradas
            foreach (PropertyInfo prop in EntityType.GetProperties().Where(p => p.GetCustomAttribute<IgnoredPropertyAttribute>() == null && Consts.IsValidPropertyType(p.PropertyType))) {
                PropertyDescriptor descriptor = new PropertyDescriptor(prop);
                Properties.Add(descriptor.ColumnName, descriptor);
            }
        }

        /// <summary>
        /// Obtiene las relaciones a otras entidades
        /// </summary>
        private void GetRelationships() {
            //Recorremos las propiedades que no están ignoradas y que no están dentro de los tipos válidos de entidades
            foreach (PropertyInfo prop in EntityType.GetProperties().Where(p => p.GetCustomAttribute<IgnoredPropertyAttribute>() == null && p.GetCustomAttributes<ForeignKeyAttribute>().Any())) {
                //Obtenemos los atributos de clave foránea
                IEnumerable<ForeignKeyAttribute> foreignKeys = prop.GetCustomAttributes<ForeignKeyAttribute>();
                foreach (ForeignKeyAttribute foreignKey in foreignKeys) {
                    PropertyInfo navigationProperty = EntityType.GetProperty(foreignKey.NavigationProperty);
                    if (navigationProperty == null)
                        throw Error.NavigationPropertyDontExistsException(foreignKey.NavigationProperty, EntityType.Name);

                    Type navigationPropertyType = Nullable.GetUnderlyingType(navigationProperty.PropertyType) ?? navigationProperty.PropertyType;
                    if (!Consts.IsValidNavigationPropertyType(navigationPropertyType))
                        throw Error.InvalidNavigationPropertyException(navigationProperty.Name);

                    //Buscamos la propiedad foránea
                    string foreignPropertyName = (string.IsNullOrWhiteSpace(foreignKey.ForeignProperty) ? prop.Name : foreignKey.ForeignProperty);
                    PropertyInfo foreignProperty = navigationPropertyType.GetProperty(foreignPropertyName);
                    if (foreignProperty == null)
                        throw Error.ForeignPropertyDontExistsException(foreignPropertyName, navigationPropertyType.Name);

                    Type foreignPropertyType = Nullable.GetUnderlyingType(foreignProperty.PropertyType) ?? foreignProperty.PropertyType;
                    //Validamos que las dos propiedades (local y foranea) sean del mismo tipo
                    if (!foreignPropertyType.IsAssignableFrom(prop.PropertyType))
                        throw Error.InvalidForeignPropertyException(foreignPropertyName);

                    //Comprobamos si existe ya la relación
                    if (!Relationships.Any(r => r.PrincipalEntity.IsAssignableFrom(navigationPropertyType) && r.ForeignEntity.IsAssignableFrom(EntityType))) {
                        Relationships.Add(new Relationship(navigationPropertyType, EntityType, navigationProperty, new List<PropertyPair>(new PropertyPair[] { new PropertyPair(foreignProperty, prop) })));
                    } else {
                        Relationship relationship = Relationships.First(r => r.PrincipalEntity.IsAssignableFrom(navigationPropertyType) && r.ForeignEntity.IsAssignableFrom(EntityType));
                        //Validamos que el par de propiedades no hayan sido agregadas anteriormente
                        if (!relationship.Properties.Any(p => p.PrincipalProperty.Name == navigationProperty.Name && p.ForeignProperty.Name == prop.Name))
                            relationship.Properties.Add(new PropertyPair(foreignProperty, prop));
                    }
                }
            }
        }

        #endregion
    }
}
