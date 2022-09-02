using OpheliaSuiteV2.Core.DataAccess.MicroSQL.Attributes;
using OpheliaSuiteV2.Core.DataAccess.MicroSQL.ORM.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace OpheliaSuiteV2.Core.DataAccess.MicroSQL.ORM {

    /// <summary>
    /// Envuelve una entidad
    /// manejando su estado
    /// </summary>
    public sealed class EntityEntry {

        #region Properties

        /// <summary>
        /// Llave de la entidad
        /// </summary>
        internal EntityKey Key {
            get; private set;
        }

        /// <summary>
        /// Descriptor de la entidad
        /// </summary>
        internal EntityDescriptor Descriptor { get; private set; }

        /// <summary>
        /// Valores originales de la entidad cuando se consultó
        /// </summary>
        public EntityStateSnapshot OriginalValues {
            get; private set;
        }

        /// <summary>
        /// Obtiene los valores actuales de la entidad
        /// </summary>
        public EntityStateSnapshot CurrentValues {
            get {
                return new EntityStateSnapshot(Entity);
            }
        }

        /// <summary>
        /// Entidad envuelta
        /// </summary>
        public object Entity {
            get; private set;
        }

        /// <summary>
        /// Estado de la entidad
        /// </summary>
        public EntityState State {
            get; private set;
        }

        #endregion

        #region Builders

        /// <summary>
        /// Inicializa una nueva instancia de la clase
        /// </summary>
        /// <param name="entity">Instancia de la entidad</param>
        /// <param name="state">Estado de la entidad</param>
        public EntityEntry(object entity, EntityState state = EntityState.Unchanged) {
            entity = entity ?? throw Error.ArgumentException(nameof(entity));
            Descriptor = EntityDescriptorMapper.GetOrAddDescriptor(entity.GetType());
            UpdateInstance(entity, state);
        }

        #endregion

        #region Methods

        /// <summary>
        /// Actualiza la instancia de la entidad
        /// </summary>
        /// <param name="entity">Entidad</param>
        /// <param name="state">Estado</param>
        internal void UpdateInstance(object entity, EntityState state = EntityState.Unchanged) {
            Entity = entity ?? throw Error.ArgumentException(nameof(entity));
            if (OriginalValues == null)
                OriginalValues = new EntityStateSnapshot(entity);
            State = state;
            if (Key == null)
                Key = new EntityKey(this, -1);
            else
                Key.UpdateInstance(this, Key.Index);
        }

        /// <summary>
        /// Acepta los cambios realizados y se actualiza
        /// los valores originales con los valores actuales
        /// </summary>
        internal void AcceptChanges() {
            if (State == EntityState.Added || State == EntityState.Modified) {
                if (State == EntityState.Modified || State == EntityState.Added) {
                    OriginalValues = CurrentValues;
                }
                State = EntityState.Unchanged;
                Key.UpdateInstance(this, -1);
            }
        }

        /// <summary>
        /// Asigna un nuevo estado a la entidad
        /// </summary>
        /// <param name="newState">Nuevo estado a asignar</param>
        internal void SetState(EntityState newState) {
            switch (newState) {
                case EntityState.Unchanged:
                    if (State == EntityState.Modified || State == EntityState.Removed) {
                        ExecuteUnchangedState();
                    }
                    break;
                case EntityState.Modified:
                    if (State == EntityState.Unchanged || State == EntityState.Removed) {
                        ExecuteModifiedState();
                    }
                    break;
                case EntityState.Removed:
                    if (State == EntityState.Unchanged || State == EntityState.Modified) {
                        ExecuteDeletedState();
                    }
                    break;
            }
        }

        /// <summary>
        /// Ejecuta la lógica necesaria
        /// para dejar la entidad sin cambios
        /// </summary>
        private void ExecuteUnchangedState() {
            //Reasignamos los valores originales a la instancia de la entidad
            foreach (PropertySnapshot prop in OriginalValues.Properties) {
                prop.Descriptor.Property.SetValue(Entity, prop.Value);
            }
            State = EntityState.Unchanged;
        }

        /// <summary>
        /// Ejecuta la lógica necesaria
        /// para dejar la entidad modificada
        /// </summary>
        private void ExecuteModifiedState() {
            State = EntityState.Modified;
        }

        /// <summary>
        /// Ejecuta la lógica necesaria
        /// para dejar la entidad eliminada
        /// </summary>
        private void ExecuteDeletedState() {
            State = EntityState.Removed;
        }

        #endregion
    }

    /// <summary>
    /// Estado de seguimiento de la entidad
    /// </summary>
    public enum EntityState {
        /// <summary>
        /// Sin cambios
        /// </summary>
        Unchanged = 0,
        /// <summary>
        /// Agregada nueva
        /// </summary>
        Added,
        /// <summary>
        /// Modificada
        /// </summary>
        Modified,
        /// <summary>
        /// Eliminada
        /// </summary>
        Removed
    }
}
