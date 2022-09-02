using OpheliaSuiteV2.Core.DataAccess.MicroSQL.Attributes;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace OpheliaSuiteV2.Core.DataAccess.MicroSQL.ORM.Internal {

    /// <summary>
    /// Encapsula los datos de la llave primaria de
    /// una entidad, creando un UId para identificar la entidad
    /// </summary>
    internal sealed class EntityKey {

        #region Properties

        /// <summary>
        /// Indice de la llave en el
        /// administrador de estados
        /// </summary>
        public long Index {
            get; set;
        }

        /// <summary>
        /// Id de la instancia
        /// </summary>
        public int InstanceId {
            get; private set;
        }

        /// <summary>
        /// Tipo de la llave
        /// </summary>
        public KeyType Type {
            get; private set;
        }

        /// <summary>
        /// Propiedades que componen la llave
        /// </summary>
        public IReadOnlyList<PropertySnapshot> Properties {
            get; private set;
        }

        /// <summary>
        /// Entrada de entidad que define la llave
        /// </summary>
        public EntityEntry Entry {
            get; private set;
        }

        #endregion

        #region Builders

        /// <summary>
        /// Inicializa una nueva instancia de la clase
        /// </summary>
        /// <param name="entry">Entrada de entidad que define la llave</param>
        /// <param name="index">Indice de la llave en el administrador de estados</param>
        public EntityKey(EntityEntry entry, long index) {
            UpdateInstance(entry, index);
        }

        #endregion

        #region Methods

        /// <summary>
        /// Actualiza la instancia de la entidad
        /// </summary>
        /// <param name="entry">Entidad</param>
        /// <param name="index">Indice</param>
        public void UpdateInstance(EntityEntry entry, long index) {
            Entry = entry;
            Index = index;
            GetKeyProperties();
            InstanceId = RuntimeHelpers.GetHashCode(Entry.Entity);
        }

        /// <summary>
        /// Obtiene las propiedades marcada como llave de la entidad
        /// </summary>
        /// <returns>Lista de propiedades</returns>
        private void GetKeyProperties() {
            Properties = Entry.OriginalValues.Properties.Where(p => p.Descriptor.IsKey || p.Descriptor.IsKeyPart).ToList();
            Type = KeyType.Manually;
            foreach (PropertySnapshot prop in Properties.Where(p => p.Descriptor.IsKey)) {
                KeyAttribute key = Entry.Entity.GetType().GetProperty(prop.Descriptor.Name).GetCustomAttribute<KeyAttribute>();
                if (key != null) {
                    Type = key.Type;
                }
                break;
            }
        }

        #endregion
    }
}
