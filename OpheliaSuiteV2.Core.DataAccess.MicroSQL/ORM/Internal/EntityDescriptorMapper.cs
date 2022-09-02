using System;
using System.Collections.Concurrent;

namespace OpheliaSuiteV2.Core.DataAccess.MicroSQL.ORM.Internal {

    /// <summary>
    /// Mapeador de descripciones de entidades
    /// </summary>
    internal static class EntityDescriptorMapper {

        #region Properties

        /// <summary>
        /// Mapa de descriptores de entidades
        /// </summary>
        private static ConcurrentDictionary<Type, EntityDescriptor> Descriptors { get; } = new ConcurrentDictionary<Type, EntityDescriptor>();

        #endregion

        #region Methods

        /// <summary>
        /// Obtiene o agrega un descriptor para un tipo de entidad
        /// </summary>
        /// <param name="entityType">Tipo de entidad</param>
        /// <returns>Descriptor de la entidad</returns>
        public static EntityDescriptor GetOrAddDescriptor(Type entityType) {
            return Descriptors.GetOrAdd(entityType, (type) => new EntityDescriptor(type) );
        }

        #endregion
    }
}
