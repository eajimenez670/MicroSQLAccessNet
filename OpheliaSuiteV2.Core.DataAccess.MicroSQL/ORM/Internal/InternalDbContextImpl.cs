using System.ComponentModel;

namespace OpheliaSuiteV2.Core.DataAccess.MicroSQL.ORM.Internal {

    /// <summary>
    /// Implementación interna de <see cref="DbContext"/>
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public sealed class InternalDbContextImpl : DbContext {

        #region Builders

        /// <summary>
        /// Inicializa una nueva instancia de la clase
        /// </summary>
        public InternalDbContextImpl() : this(new DbContextOptions()) { }

        /// <summary>
        /// Inicializa una nueva instancia de la clase
        /// </summary>
        /// <param name="options">Opciones de construcción</param>
        public InternalDbContextImpl(DbContextOptions options) : base(options) {
        }

        #endregion
    }
}
