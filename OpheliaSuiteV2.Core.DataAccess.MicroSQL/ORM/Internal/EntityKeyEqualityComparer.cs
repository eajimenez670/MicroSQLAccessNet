using System.Collections.Generic;
using System.Linq;

namespace OpheliaSuiteV2.Core.DataAccess.MicroSQL.ORM.Internal {

    /// <summary>
    /// Comparador de llaves de entidad
    /// </summary>
    internal sealed class EntityKeyEqualityComparer : IEqualityComparer<EntityKey> {

        #region Properties

        /// <summary>
        /// Obtiene la unica instancia de la clase
        /// </summary>
        public static EntityKeyEqualityComparer Instance {
            get;
        } = new EntityKeyEqualityComparer();

        #endregion

        #region Builders

        /// <summary>
        /// Inicializa una nueva instancia de la clase
        /// </summary>
        private EntityKeyEqualityComparer() {
        }

        #endregion

        #region IEqualityComparer

        public bool Equals(EntityKey x, EntityKey y) {
            if (object.ReferenceEquals(x.Entry.Entity, y.Entry.Entity))
                return true;

            if (!x.Entry.Entity.GetType().IsAssignableFrom(y.Entry.Entity.GetType()))
                return false;

            if (x.Properties.Count != y.Properties.Count)
                return false;

            if (x.Type != y.Type)
                return false;

            foreach (PropertySnapshot propX in x.Properties) {
                PropertySnapshot propY = y.Properties.FirstOrDefault(p => p.Descriptor.Name == propX.Descriptor.Name);
                if (propY == null)
                    return false;
                if (!propX.Descriptor.Property.PropertyType.IsAssignableFrom(propY.Descriptor.Property.PropertyType))
                    return false;
                if (propX.Value != propY.Value)
                    return false;
                if (propX.Descriptor.ColumnName != propY.Descriptor.ColumnName)
                    return false;
            }

            return true;
        }

        public int GetHashCode(EntityKey obj) => obj.InstanceId;

        #endregion
    }
}
