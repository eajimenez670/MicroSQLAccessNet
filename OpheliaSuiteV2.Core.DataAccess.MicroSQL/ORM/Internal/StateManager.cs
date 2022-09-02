using OpheliaSuiteV2.Core.DataAccess.MicroSQL.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace OpheliaSuiteV2.Core.DataAccess.MicroSQL.ORM.Internal {

    /// <summary>
    /// Administra el estado de las entidades
    /// </summary>
    internal sealed class StateManager {

        #region Fields

        /// <summary>
        /// Candado para bloqueo de codigo
        /// </summary>
        private readonly object _lock = new object();

        /// <summary>
        /// Indice actual de entidades
        /// </summary>
        private long _currentIndex = 0;

        /// <summary>
        /// Referencias mapeadas en el contexto
        /// </summary>
        private readonly Dictionary<EntityKey, EntityEntry> _referenceMap = new Dictionary<EntityKey, EntityEntry>(EntityKeyEqualityComparer.Instance);

        /// <summary>
        /// Mapa de relaciones entre entidades
        /// </summary>
        private readonly Dictionary<Type, List<Relationship>> _relationshipMap = new Dictionary<Type, List<Relationship>>();

        #endregion

        #region Builders

        /// <summary>
        /// Inicializa una nueva instancia de la clase
        /// </summary>
        public StateManager() {
        }

        #endregion

        #region Methods

        /// <summary>
        /// Trata de obtener la relación entre dos entidades
        /// </summary>
        /// <typeparam name="TPrincipal">Tipo de la entidad principal</typeparam>
        /// <typeparam name="TForeign">Tipo de la entidad foránea</typeparam>
        /// <param name="relationship">Relación obtenida</param>
        /// <returns>Valor que indica si se obtuvo la relación</returns>
        public bool TryGetRelationship<TPrincipal, TForeign>(out Relationship relationship) {
            return TryGetRelationship(typeof(TPrincipal), typeof(TForeign), out relationship);
        }

        /// <summary>
        /// Trata de obtener la relación entre dos entidades
        /// </summary>
        /// <param name="principalEntity">Tipo de la entidad principal</param>
        /// <param name="foreignEntity">Tipo de la entidad foránea</param>
        /// <param name="relationship">Relación obtenida</param>
        /// <returns>Valor que indica si se obtuvo la relación</returns>
        public bool TryGetRelationship(Type principalEntity, Type foreignEntity, out Relationship relationship) {
            relationship = null;
            if (_relationshipMap.ContainsKey(principalEntity)) {
                List<Relationship> relationships = _relationshipMap[principalEntity];
                relationship = relationships.FirstOrDefault(r => r.ForeignEntity.IsAssignableFrom(foreignEntity));
                return true;
            }
            return false;
        }

        /// <summary>
        /// Detecta cambios en las entidades marcadas como <see cref="EntityState.Unchanged"/>
        /// </summary>
        private void DetectChanges() {
            lock (_lock) {
                foreach (EntityEntry entry in GetUnchangedEntities()) {
                    if (entry.OriginalValues.FootPrint != entry.CurrentValues.FootPrint)
                        entry.SetState(EntityState.Modified);
                }
            }
        }

        /// <summary>
        /// Agrega una nueva entrada con un estado espesífico
        /// </summary>
        /// <param name="entity">Entidad a agregar</param>
        /// <param name="state">Estado de la entidad</param>
        public void AddOrUpdateEntry(object entity, EntityState state = EntityState.Unchanged) {
            lock (_lock) {
                //Verificamos si ya existe la instancia de la entidad en el mapa de referencias
                EntityEntry entry = new EntityEntry(entity, state);
                long index = 0;
                if (_referenceMap.ContainsKey(entry.Key)) {
                    entry = _referenceMap[entry.Key];
                    string entityName = entry.Entity.GetType().Name;
                    switch (entry.State) {
                        case EntityState.Unchanged:
                            if (state == EntityState.Added)
                                throw Error.InvalidOperationEntityEntryException(entityName);
                            else
                                UpdateInstanceEntry(entry, entity, state);
                            return;
                        case EntityState.Modified:
                            if (state == EntityState.Added)
                                throw Error.InvalidOperationEntityEntryException(entityName);
                            else if (state == EntityState.Unchanged)
                                entry.SetState(state);
                            else
                                UpdateInstanceEntry(entry, entity, state);
                            return;
                        case EntityState.Removed:
                            if (state == EntityState.Added || state == EntityState.Modified)
                                throw Error.InvalidOperationEntityEntryException(entityName);
                            else if (state == EntityState.Unchanged)
                                entry.SetState(state);
                            else
                                UpdateInstanceEntry(entry, entity, state);
                            return;
                        case EntityState.Added:
                            if (state == EntityState.Added)
                                UpdateInstanceEntry(entry, entity, state);
                            else
                                throw Error.InvalidOperationEntityEntryException(entityName);
                            return;
                    }
                    EnsureRelationships(entry);
                } else {
                    index = GetNextIndex();
                    entry.Key.Index = index;
                    EnsureRelationships(entry);
                    _referenceMap.Add(entry.Key, entry);
                }
            }
        }

        /// <summary>
        /// Actualiza los datos de los campos relacionados
        /// </summary>
        /// <param name="entry">Entidad principal</param>
        public void UpdateRelationshipsData(EntityEntry entry) {
            //Si existe relaciones con la entidad cuando la entidad es principal
            if (_relationshipMap.ContainsKey(entry.Entity.GetType())) {
                //Buscamos las referencias de las entidades hijas y las aculizamos
                List<Relationship> relationships = _relationshipMap[entry.Entity.GetType()];
                foreach (Relationship relationship in relationships) {
                    foreach (KeyValuePair<EntityKey, EntityEntry> keyValueEntry in _referenceMap.Where(kv => kv.Value.Entity.GetType().IsAssignableFrom(relationship.ForeignEntity))) {
                        //Se actualiza las propiedades foráneas de la entidad hija,
                        //si las dos entidades está relacionadas por la propiedad de navegación
                        if (ReferenceEquals(relationship.NavigationProperty.GetValue(keyValueEntry.Value.Entity), entry.Entity)) {
                            foreach (PropertyPair propertyPair in relationship.Properties) {
                                propertyPair.ForeignProperty.SetValue(keyValueEntry.Value.Entity, propertyPair.PrincipalProperty.GetValue(entry.Entity));
                            }
                        }
                        //Si la propiedad de navegación en la entidad relacionada está nula,
                        //Se valida que si las propiedades foráneas de la relación son iguales
                        //en ambas entidades, se le asigna la entidad actual a la propiedad de navegación
                        if (relationship.NavigationProperty.GetValue(keyValueEntry.Value.Entity) == null) {
                            bool hasRelation = true;
                            foreach (PropertyPair propertyPair in relationship.Properties) {
                                if (!propertyPair.ForeignProperty.GetValue(keyValueEntry.Value.Entity).Equals(propertyPair.PrincipalProperty.GetValue(entry.Entity))) {
                                    hasRelation = false;
                                    break;
                                }
                            }
                            //Si tiene relación se asigna la instancia de la entidad actual como propiedad de navegación
                            if (hasRelation) {
                                relationship.NavigationProperty.SetValue(keyValueEntry.Value.Entity, entry.Entity);
                            }
                        }
                    }
                }
            }
            //Si existe relaciones con la entidad cuando la entidad es hija
            //Obtengo las propiedades de navegación que son referencia a la entidad padre
            List<PropertyInfo> navigationProperties = entry.Entity.GetType().GetProperties().Where(p => p.GetCustomAttribute<IgnoredPropertyAttribute>() == null && Consts.IsValidNavigationPropertyType(p.PropertyType)).ToList();
            foreach (PropertyInfo prop in navigationProperties) {
                if (prop.GetValue(entry.Entity) != null) {
                    if (_relationshipMap.ContainsKey(prop.PropertyType)) {
                        List<Relationship> relationships = _relationshipMap[prop.PropertyType].Where(r => r.ForeignEntity.IsAssignableFrom(entry.Entity.GetType())).ToList();
                        foreach (Relationship relationship in relationships) {
                            foreach (PropertyPair propertyPair in relationship.Properties) {
                                propertyPair.ForeignProperty.SetValue(entry.Entity, propertyPair.PrincipalProperty.GetValue(prop.GetValue(entry.Entity)));
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Asegura las relaciones encontradas en la entrada de la entidad
        /// </summary>
        /// <param name="entry">Entrada de la entidad</param>
        private void EnsureRelationships(EntityEntry entry) {
            //Validamos si ya existe una entrada en el mapa para las
            //entidades principales en las relaciones de la entrada de la entidad
            foreach (Relationship relationship in entry.Descriptor.Relationships) {
                if (_relationshipMap.ContainsKey(relationship.PrincipalEntity)) {
                    //Validamos si ya tiene una entrada para la entidad foránea
                    if (!_relationshipMap[relationship.PrincipalEntity].Any(r => r.ForeignEntity.IsAssignableFrom(relationship.ForeignEntity)))
                        _relationshipMap[relationship.PrincipalEntity].Add(relationship);
                } else {
                    _relationshipMap.Add(relationship.PrincipalEntity, new List<Relationship>(new Relationship[] { relationship }));
                }
            }
            UpdateRelationshipsData(entry);
        }

        /// <summary>
        /// Actualiza el estado de una entrada de entidad así
        /// como su instancia y valores
        /// </summary>
        /// <param name="entry">Entrada de entidad</param>
        /// <param name="entity">Nueva instancia</param>
        /// <param name="state">Nuevo estado</param>
        private void UpdateInstanceEntry(EntityEntry entry, object entity, EntityState state) {
            entry.UpdateInstance(entity, state);
            entry.Key.Index = GetNextIndex();
        }

        /// <summary>
        /// Obtiene el proximo indice
        /// </summary>
        /// <returns>Proximo indice</returns>
        private long GetNextIndex() {
            lock (_lock) {
                return _currentIndex++;
            }
        }

        /// <summary>
        /// Limpia todas las entradas en el administrador
        /// de estados
        /// </summary>
        public void CleanEntries() {
            lock (_lock) {
                _referenceMap.Clear();
            }
        }

        /// <summary>
        /// Descarta todos los cambios que se hayan realizado
        /// sobre las entidades en el contexto
        /// </summary>
        public void DiscardChanges() {
            lock (_lock) {
                //Removemos las entidades agregadas
                List<EntityKey> addedEntities = new List<EntityKey>();
                addedEntities.AddRange(_referenceMap.Where(kv => kv.Value.State == EntityState.Added).Select(kv => kv.Key).ToList());
                addedEntities.ForEach(key => _referenceMap.Remove(key));
                //Reestablecemos los valores de las entidades eliminada y modificadas
                _referenceMap.Values.ToList().ForEach(entity => entity.SetState(EntityState.Unchanged));
            }
        }

        /// <summary>
        /// Acepta los cambios realizados en las entidades.
        /// Al hacerlo, las entidades agregadas y modificadas pasan a estar sin camnios
        /// y las eliminadas son quitadas de mapeador de referencias
        /// </summary>
        public void AcceptChanges() {
            lock (_lock) {
                //Removemos las entidades eliminadas
                List<EntityKey> removedEntities = new List<EntityKey>();
                removedEntities.AddRange(_referenceMap.Where(kv => kv.Value.State == EntityState.Removed).Select(kv => kv.Key).ToList());
                removedEntities.ForEach(key => _referenceMap.Remove(key));
                //Aceptamos los cambios en las entidades agregadas y modificadas
                _referenceMap.Values.ToList().ForEach(entity => entity.AcceptChanges());
                _currentIndex = 0;
            }
        }

        /// <summary>
        /// Obtiene una lista de las entidades sin cambios
        /// en el administrador de estados
        /// </summary>
        /// <returns>Lista de entidades</returns>
        public List<EntityEntry> GetUnchangedEntities() {
            lock (_lock) {
                return _referenceMap.Values.Where(entry => entry.State == EntityState.Unchanged).ToList();
            }
        }

        /// <summary>
        /// Obtiene una lista de las entidades para inserción
        /// en el administrador de estados
        /// </summary>
        /// <returns>Lista de entidades</returns>
        public List<EntityEntry> GetAddedEntities() {
            lock (_lock) {
                return _referenceMap.Values.Where(entry => entry.State == EntityState.Added).ToList();
            }
        }

        /// <summary>
        /// Obtiene una lista de las entidades marcadas como modificadas
        /// en el administrador de estados
        /// </summary>
        /// <returns>Lista de entidades</returns>
        public List<EntityEntry> GetModifiedEntities() {
            lock (_lock) {
                return _referenceMap.Values.Where(entry => entry.State == EntityState.Modified).ToList();
            }
        }

        /// <summary>
        /// Obtiene una lista de las entidades marcadas para eliminación
        /// en el administrador de estados
        /// </summary>
        /// <returns>Lista de entidades</returns>
        public List<EntityEntry> GetDeletedEntities() {
            lock (_lock) {
                return _referenceMap.Values.Where(entry => entry.State == EntityState.Removed).ToList();
            }
        }

        /// <summary>
        /// Obtiene la lista de las entidade para
        /// procesar en la transacción en orden de ejecución
        /// </summary>
        /// <returns>Lista de entidades</returns>
        public List<EntityEntry> GetEntitiesToProcess() {
            lock (_lock) {
                DetectChanges();
                return _referenceMap
                    .Where(kv => kv.Value.State != EntityState.Unchanged)
                    .OrderBy(kv => kv.Key.Index)
                    .Select(kv => kv.Value).ToList();
            }
        }

        #endregion
    }
}
