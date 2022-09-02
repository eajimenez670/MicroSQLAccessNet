using OpheliaSuiteV2.Core.DataAccess.MicroSQL.ORM.Internal;
using OpheliaSuiteV2.Core.DataAccess.MicroSQL.ORM.Internal.LinqToSql;
using OpheliaSuiteV2.Core.DataAccess.MicroSQL.QueryBuilder;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace OpheliaSuiteV2.Core.DataAccess.MicroSQL.ORM {

    /// <summary>
    /// Repositorio base
    /// </summary>
    /// <typeparam name="TEntity">Tipo de la entidad del repositorio</typeparam>
    public abstract class Repository<TEntity> : IRepository<TEntity> where TEntity : class, new() {

        #region Fields

        /// <summary>
        /// Descriptor de la entidad
        /// </summary>
        private readonly EntityDescriptor _descriptor;
        ///// <summary>
        ///// Convertidor de expresiones
        ///// </summary>
        //private LinqToSqlExpressionVisitor<TEntity> _linqToSqlExpression;

        #endregion

        #region Properties

        /// <summary>
        /// Contexto de datos
        /// </summary>
        public AbstractDbContext Context {
            get; private set;
        }

        #endregion

        #region Builders

        /// <summary>
        /// Inicializa una nueva instancia de la clase
        /// </summary>
        public Repository() {
            _descriptor = EntityDescriptorMapper.GetOrAddDescriptor(typeof(TEntity));
        }

        /// <summary>
        /// Inicializa una nueva instancia de la clase
        /// </summary>
        /// <param name="context">Contexto de datos</param>
        public Repository(AbstractDbContext context) : this() {
            Initialize(context);
        }

        #endregion

        #region Methods

        /// <summary>
        /// Inicializa el repositorio
        /// </summary>
        /// <param name="context">Contexto de datos</param>
        public void Initialize(AbstractDbContext context) {
            Context = context;
            //_linqToSqlExpression = new LinqToSqlExpressionVisitor<TEntity>(Context.InternalDbContext.Builder.SqlCompiler);
        }

        /// <summary>
        /// Asegura que el repositorio haya sido
        /// inicializado
        /// </summary>
        private void EnsureInitialized() {
            if (Context == null) throw Error.NotInitializedRepositoryException(_descriptor.EntityType.Name);
        }

        #endregion

        #region Commands

        /// <summary>
        /// Agrega una nueva entidad en el repositorio de datos
        /// </summary>
        /// <param name="entity">Entidad a agregar</param>
        /// <returns>Entidad agregada</returns>
        public TEntity Add(TEntity entity) {
            EnsureInitialized();
            Context.AttachEntities(new object[] {
                entity
            }, EntityState.Added);
            return entity;
        }

        /// <summary>
        /// Modifica una entidad en el repositorio de datos
        /// </summary>
        /// <param name="entity">Entidad a modificar</param>
        /// <returns>Entidad modificada</returns>
        public TEntity Modify(TEntity entity) {
            EnsureInitialized();
            Context.AttachEntities(new object[] {
                entity
            }, EntityState.Modified);
            return entity;
        }

        /// <summary>
        /// Remueve una entidad en el repositorio de datos
        /// </summary>
        /// <param name="entity">Entidad a remover</param>
        /// <returns>Entidad removida</returns>
        public TEntity Remove(TEntity entity) {
            EnsureInitialized();
            Context.AttachEntities(new object[] {
                entity
            }, EntityState.Removed);
            return entity;
        }

        /// <summary>
        /// Agrega una nueva entidad en el repositorio de datos de forma asíncrona
        /// </summary>
        /// <param name="entity">Entidad a agregar</param>
        /// <returns>Entidad agregada</returns>
        public Task<TEntity> AddAsync(TEntity entity) {
            return Task.Factory.StartNew(() => Add(entity), new CancellationToken(false), TaskCreationOptions.LongRunning, TaskScheduler.Default);
        }

        /// <summary>
        /// Modifica una entidad en el repositorio de datos de forma asíncrona
        /// </summary>
        /// <param name="entity">Entidad a modificar</param>
        /// <returns>Entidad modificada</returns>
        public Task<TEntity> ModifyAsync(TEntity entity) {
            return Task.Factory.StartNew(() => Modify(entity), new CancellationToken(false), TaskCreationOptions.LongRunning, TaskScheduler.Default);
        }

        /// <summary>
        /// Remueve una entidad en el repositorio de datos de forma asíncrona
        /// </summary>
        /// <param name="entity">Entidad a remover</param>
        /// <returns>Entidad removida</returns>
        public Task<TEntity> RemoveAsync(TEntity entity) {
            return Task.Factory.StartNew(() => Remove(entity), new CancellationToken(false), TaskCreationOptions.LongRunning, TaskScheduler.Default);
        }

        #endregion

        #region Query

        /// <summary>
        /// Lista todas las entidades en el repositorio de datos
        /// </summary>
        /// <returns>Conjunto de entidades resultado</returns>
        public ResultSet<TEntity> List() {
            EnsureInitialized();
            Query query = new Query(_descriptor.TableName);
            var sqlResult = Context.InternalDbContext.Builder.SqlCompiler.Compile(query.Select(_descriptor.Properties.Select(kv => kv.Key).ToArray()));
            IEnumerable<TEntity> result = Context.InternalDbContext.ExecuteQuery<TEntity>(sqlResult.Sql).ToList();
            Context.AttachEntities(result, EntityState.Unchanged);
            return new ResultSet<TEntity>(Context, result);
        }

        /// <summary>
        /// Lista las entidades que cumplen con la expresión
        /// </summary>
        /// <param name="expression">Expresión de búsqueda</param>
        /// <returns>Conjunto de entidades resultado</returns>
        [Obsolete("Aún no se ha implementado")]
        public ResultSet<TEntity> List(Expression<Func<TEntity, bool>> expression) {
            EnsureInitialized();
            throw new NotImplementedException();
        }

        /// <summary>
        /// Lista las entidades en el repositorio de datos que cumplan con la condición <paramref name="rawWhere"/>
        /// </summary>
        /// <param name="rawWhere">Condición WHERE cruda sin incluir la palabra reservada WHERE</param>
        /// <param name="parameters">Objeto de parámetros usados en la condición</param>
        /// <returns>Conjunto de entidades resultado</returns>
        public ResultSet<TEntity> List(string rawWhere, object parameters = null) {
            EnsureInitialized();
            if (string.IsNullOrWhiteSpace(rawWhere))
                throw Error.ArgumentException(nameof(rawWhere));

            Query query = new Query(_descriptor.TableName);
            rawWhere = (rawWhere.Trim().ToUpper(CultureInfo.InvariantCulture).StartsWith("WHERE", StringComparison.InvariantCulture) ? rawWhere.Trim() : $"WHERE {rawWhere.Trim()}");
            string sql = $"{Context.InternalDbContext.Builder.SqlCompiler.Compile(query.Select(_descriptor.Properties.Select(kv => kv.Key).ToArray())).Sql} {rawWhere}";
            IEnumerable<TEntity> result = Context.InternalDbContext.ExecuteQuery<TEntity>(sql, parameters).ToList();
            Context.AttachEntities(result, EntityState.Unchanged);
            return new ResultSet<TEntity>(Context, result);
        }

        /// <summary>
        /// Lista todas las entidades en el repositorio de datos
        /// </summary>
        /// <returns>Conjunto de entidades resultado</returns>
        public Task<ResultSet<TEntity>> ListAsync() {
            return Task.Factory.StartNew(List, new CancellationToken(false), TaskCreationOptions.LongRunning, TaskScheduler.Default);
        }

        /// <summary>
        /// Lista las entidades que cumplen con la expresión
        /// </summary>
        /// <param name="expression">Expresión de búsqueda</param>
        /// <returns>Conjunto de entidades resultado</returns>
        [Obsolete("Aún no se ha implementado")]
        public Task<ResultSet<TEntity>> ListAsync(Expression<Func<TEntity, bool>> expression) {
            return Task.Factory.StartNew(() => List(expression), new CancellationToken(false), TaskCreationOptions.LongRunning, TaskScheduler.Default);
        }

        /// <summary>
        /// Lista las entidades en el repositorio de datos que cumplan con la condición <paramref name="rawWhere"/>
        /// </summary>
        /// <param name="rawWhere">Condición WHERE cruda</param>
        /// <param name="parameters">Objeto de parámetros usados en la condición</param>
        /// <returns>Conjunto de entidades resultado</returns>
        public Task<ResultSet<TEntity>> ListAsync(string rawWhere, object parameters = null) {
            return Task.Factory.StartNew(() => List(rawWhere, parameters), new CancellationToken(false), TaskCreationOptions.LongRunning, TaskScheduler.Default);
        }

        #endregion
    }
}
