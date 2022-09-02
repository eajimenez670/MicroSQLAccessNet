using System.Collections;
using System.Collections.Generic;

namespace OpheliaSuiteV2.Core.DataAccess.MicroSQL.ORM {

    /// <summary>
    /// Encapsula el conjunto de resultados de la ejeución de una
    /// secuencia de comandos
    /// </summary>
    /// <typeparam name="TEntity">Tipo de la entidad resultado</typeparam>
    public sealed class ResultSet<TEntity> : IEnumerable<TEntity> where TEntity : class, new() {

        #region Properties

        /// <summary>
        /// Resultado interno
        /// </summary>
        internal IEnumerable<TEntity> Result { get; private set; }
        /// <summary>
        /// Contexto de datos
        /// </summary>
        internal AbstractDbContext Context { get; private set; }

        #endregion

        #region Builders

        /// <summary>
        /// Inicializa una nueva instancia de la clase
        /// </summary>
        /// <param name="context">Contexto de datos</param>
        /// <param name="entities">Entidades resultado</param>
        internal ResultSet(AbstractDbContext context, IEnumerable<TEntity> entities) {
            Context = context ?? throw Error.ArgumentException(nameof(context));
            Result = entities ?? throw Error.ArgumentException(nameof(entities));
        }

        #endregion

        #region IEnumerable

        /// <summary>
        /// Obtiene un enumerador
        /// </summary>
        /// <returns>Enumerador</returns>
        public IEnumerator<TEntity> GetEnumerator() {
            return Result.GetEnumerator();
        }

        /// <summary>
        /// Obtiene un enumerador
        /// </summary>
        /// <returns>Enumerador</returns>
        IEnumerator IEnumerable.GetEnumerator() {
            return Result.GetEnumerator();
        }

        #endregion
    }
}
