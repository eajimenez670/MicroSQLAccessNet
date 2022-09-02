using OpheliaSuiteV2.Core.DataAccess.MicroSQL.ORM.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace OpheliaSuiteV2.Core.DataAccess.MicroSQL.ORM {

    /// <summary>
    /// Clase base que encapsula un contexto de datos de negocio
    /// </summary>
    public abstract class AbstractDbContext : IDisposable {

        #region Properties

        /// <summary>
        /// Administrador de estados de entidades
        /// </summary>
        internal StateManager StateManager { get; private set; } = new StateManager();

        private readonly DbContext _internalDbContext;
        /// <summary>
        /// Contexto interno
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public DbContext InternalDbContext => GetDbContext(false);

        #endregion

        #region Builders

        /// <summary>
        /// Inicializa una nueva instancia de la clase
        /// </summary>
        protected AbstractDbContext() : this(new DbContextOptions()) {
        }

        /// <summary>
        /// Inicializa una nueva instancia de la clase
        /// </summary>
        /// <param name="options">Opciones de construcción</param>
        protected AbstractDbContext(DbContextOptions options) {
            _internalDbContext = new InternalDbContextImpl(options) { UseEntityDescriptor = true };
        }

        #endregion

        #region Methods

        /// <summary>
        /// Obtiene el contexto interno
        /// </summary>
        /// <param name="useEntityDescriptor">Valor que indica si se usa el descriptor de entidades
        /// en el proceso de serialización</param>
        /// <returns>Contexto interno</returns>
        private DbContext GetDbContext(bool useEntityDescriptor = true) {
            _internalDbContext.UseEntityDescriptor = useEntityDescriptor;
            return _internalDbContext;
        }

        /// <summary>
        /// Persiste los cambios realizados en las
        /// entidades manejadas en el contexto
        /// </summary>
        public void SaveChanges() {
            //Iniciamos una transacción
            GetDbContext().BeginTransaction();
            //Creamos los comandos a ejecutar
            foreach (CommandDefinition command in CommandDefinitionBuilder.Build(StateManager, InternalDbContext, true)) {
                GetDbContext().ExecuteNonQuery(command.Command);
                StateManager.UpdateRelationshipsData(command.Entry);
            }
            GetDbContext().CommitChanges();
            StateManager.AcceptChanges();
        }

        /// <summary>
        /// Persiste los cambios realizados en las
        /// entidades manejadas en el contexto de forma asíncrona
        /// </summary>
        public Task SaveChangesAsync() {
            return Task.Factory.StartNew(SaveChanges);
        }

        /// <summary>
        /// Adjunta entidades al administrador de estados
        /// </summary>
        /// <param name="entities">Entidades</param>
        /// <param name="state">Estado con que se adjuntan las entidades</param>
        internal void AttachEntities(IEnumerable<object> entities, EntityState state) {
            foreach (object entity in entities) {
                StateManager.AddOrUpdateEntry(entity, state);
            }
        }

        #endregion

        #region IDisposable

        /// <summary>
        /// Destruye la instancia
        /// </summary>
        public void Dispose() {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Destruye la instancia
        /// </summary>
        /// <param name="disposing">Valor que indica si se esta destruyendo</param>
        protected virtual void Dispose(bool disposing) {
            if (disposing) {
                StateManager?.CleanEntries();
                InternalDbContext?.Dispose();
            }
        }

        #endregion
    }
}
