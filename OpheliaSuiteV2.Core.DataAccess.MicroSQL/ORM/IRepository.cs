using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace OpheliaSuiteV2.Core.DataAccess.MicroSQL.ORM {

    /// <summary>
    /// Define los atributos y comportamientos de un repositorio
    /// </summary>
    public interface IRepository {

        #region Properties

        /// <summary>
        /// Obtiene el contexto de datos de negocio
        /// </summary>
        AbstractDbContext Context {
            get;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Inicializa el repositorio con el contexto
        /// </summary>
        /// <param name="context">Contexto de datos al que pertenece el repositorio</param>
        void Initialize(AbstractDbContext context);

        #endregion
    }

    /// <summary>
    /// Define los atributos y comportamientos de un repositorio genérico
    /// </summary>
    /// <typeparam name="TEntity">Tipo de la entidad de negocio</typeparam>
    public interface IRepository<TEntity> : IRepository where TEntity : class, new() {

        #region Methods

        /// <summary>
        /// Agrega una nueva entidad en el repositorio de datos
        /// </summary>
        /// <param name="entity">Entidad a agregar</param>
        /// <returns>Entidad agregada</returns>
        TEntity Add(TEntity entity);

        /// <summary>
        /// Modifica una entidad en el repositorio de datos
        /// </summary>
        /// <param name="entity">Entidad a modificar</param>
        /// <returns>Entidad modificada</returns>
        TEntity Modify(TEntity entity);

        /// <summary>
        /// Remueve una entidad en el repositorio de datos
        /// </summary>
        /// <param name="entity">Entidad a remover</param>
        /// <returns>Entidad removida</returns>
        TEntity Remove(TEntity entity);

        /// <summary>
        /// Lista todas las entidades en el repositorio de datos
        /// </summary>
        /// <returns>Conjunto de resultados</returns>
        ResultSet<TEntity> List();

        /// <summary>
        /// Lista las entidades que cumplen con la expresión
        /// </summary>
        /// <param name="expression">Expresión de búsqueda</param>
        /// <returns>Conjunto de resultados</returns>
        [Obsolete("Aún no se ha implementado")]
        ResultSet<TEntity> List(Expression<Func<TEntity, bool>> expression);

        /// <summary>
        /// Lista las entidades en el repositorio de datos que cumplan con la condición <paramref name="rawWhere"/>
        /// </summary>
        /// <param name="rawWhere">Condición WHERE cruda sin incluir la palabra reservada WHERE</param>
        /// <param name="parameters">Objeto de parámetros usados en la condición</param>
        /// <returns>Conjunto de resultados</returns>
        ResultSet<TEntity> List(string rawWhere, object parameters = null);

        /// <summary>
        /// Agrega una nueva entidad en el repositorio de datos de forma asíncrona
        /// </summary>
        /// <param name="entity">Entidad a agregar</param>
        /// <returns>Entidad agregada</returns>
        Task<TEntity> AddAsync(TEntity entity);

        /// <summary>
        /// Modifica una entidad en el repositorio de datos de forma asíncrona
        /// </summary>
        /// <param name="entity">Entidad a modificar</param>
        /// <returns>Entidad modificada</returns>
        Task<TEntity> ModifyAsync(TEntity entity);

        /// <summary>
        /// Remueve una entidad en el repositorio de datos de forma asíncrona
        /// </summary>
        /// <param name="entity">Entidad a remover</param>
        /// <returns>Entidad removida</returns>
        Task<TEntity> RemoveAsync(TEntity entity);

        /// <summary>
        /// Lista todas las entidades en el repositorio de datos
        /// </summary>
        /// <returns>Conjunto de resultados</returns>
        Task<ResultSet<TEntity>> ListAsync();

        /// <summary>
        /// Lista las entidades que cumplen con la expresión
        /// </summary>
        /// <param name="expression">Expresión de búsqueda</param>
        /// <returns>Conjunto de resultados</returns>
        [Obsolete("Aún no se ha implementado")]
        Task<ResultSet<TEntity>> ListAsync(Expression<Func<TEntity, bool>> expression);

        /// <summary>
        /// Lista las entidades en el repositorio de datos que cumplan con la condición <paramref name="rawWhere"/>
        /// </summary>
        /// <param name="rawWhere">Condición WHERE cruda</param>
        /// <param name="parameters">Objeto de parámetros usados en la condición</param>
        /// <returns>Conjunto de resultados</returns>
        Task<ResultSet<TEntity>> ListAsync(string rawWhere, object parameters = null);

        #endregion
    }
}
