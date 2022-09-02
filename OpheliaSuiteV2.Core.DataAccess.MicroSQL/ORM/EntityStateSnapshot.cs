using OpheliaSuiteV2.Core.DataAccess.MicroSQL.Attributes;
using OpheliaSuiteV2.Core.DataAccess.MicroSQL.ORM.Internal;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace OpheliaSuiteV2.Core.DataAccess.MicroSQL.ORM {

    /// <summary>
    /// Encapsula los valor de las proiedades de una entidad
    /// capturando su estado de datos
    /// </summary>
    public sealed class EntityStateSnapshot {

        #region Properties

        /// <summary>
        /// Obtiene el valor de una propiedad
        /// </summary>
        /// <param name="propertyName">Nombre de la propiedad</param>
        /// <returns>Valor de la propiedad</returns>
        public object this[string propertyName] {
            get {
                return GetValue(propertyName);
            }
        }

        /// <summary>
        /// Propiedades con los valores originales
        /// del estado inicial de la entidad
        /// </summary>
        internal IReadOnlyList<PropertySnapshot> Properties {
            get; private set;
        }

        /// <summary>
        /// Obtiene la huella que identifica el estado de la entidad
        /// </summary>
        public string FootPrint {
            get {
                return (string.Join("|", Properties.Select(p => p.FootPrint))).ToMD5();
            }
        }

        #endregion

        #region Builders

        /// <summary>
        /// Inicializa una nueva instancia de la clase
        /// </summary>
        /// <param name="entity">Instancia de la entidad</param>
        internal EntityStateSnapshot(object entity) {
            entity = entity ?? throw Error.ArgumentException(nameof(entity));
            UpdateInstance(entity);
        }

        #endregion

        #region Methods

        /// <summary>
        /// Actualiza la instancia de la entidad
        /// </summary>
        /// <param name="entity">Entidad</param>
        internal void UpdateInstance(object entity) {
            if (entity != null) {
                List<PropertySnapshot> props = new List<PropertySnapshot>();
                foreach (PropertyDescriptor desc in EntityDescriptorMapper.GetOrAddDescriptor(entity.GetType()).Properties.Select(p => p.Value).ToList()) {
                    props.Add(new PropertySnapshot(desc, entity));
                }
                Properties = props;
            }
        }

        /// <summary>
        /// Obtiene el valor de una propiedad
        /// </summary>
        /// <param name="propertyName">Nombre de la propiedad</param>
        /// <returns>Valor de la propiedad</returns>
        public object GetValue(string propertyName) {
            if (!string.IsNullOrWhiteSpace(propertyName)) {
                var prop = Properties.FirstOrDefault(p => p.Descriptor.Name == propertyName);
                if (prop != null) {
                    return prop.Value;
                }
            }
            return null;
        }

        /// <summary>
        /// Obtiene el valor de una propiedad
        /// </summary>
        /// <typeparam name="T">Tipo de la propiedad a obtener</typeparam>
        /// <param name="propertyName">Nombre de la propiedad</param>
        /// <returns>Valor de la propiedad</returns>
        public T GetValue<T>(string propertyName) {
            if (!string.IsNullOrWhiteSpace(propertyName)) {
                var prop = Properties.FirstOrDefault(p => p.Descriptor.Name == propertyName);
                if (prop != null) {
                    if (prop.Descriptor.Property.PropertyType.IsAssignableFrom(typeof(T)))
                        return (T)prop.Value;
                }
            }
            return default;
        }

        #endregion
    }
}
