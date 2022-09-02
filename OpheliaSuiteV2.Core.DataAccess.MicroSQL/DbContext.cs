using OpheliaSuiteV2.Core.DataAccess.MicroSQL.Builders;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Globalization;
using System.Linq;

namespace OpheliaSuiteV2.Core.DataAccess.MicroSQL {

    /// <summary>
    /// Clase base que encapsula un contexto de datos
    /// </summary>
    public abstract class DbContext : IDbContext {

        #region Properties

        /// <summary>
        /// Informa ción de la transacción en curso
        /// </summary>
        public TransactionInfo TransactionInfo {
            get; internal set;
        }

        /// <summary>
        /// Opciones de configuración del contexto
        /// </summary>
        public DbContextOptions Options {
            get; internal set;
        }

        /// <summary>
        /// Constructor de artefactos del contexto
        /// </summary>
        public DbContextBuilder Builder {
            get; internal set;
        }

        /// <summary>
        /// Valor que indica si se usa el descriptor de entidades
        /// en el proceso de serialización
        /// </summary>
        internal bool UseEntityDescriptor { get; set; }

        #endregion

        #region Builders

        /// <summary>
        /// Inicializa una nueva instancia de la clase
        /// </summary>
        public DbContext() : this(new DbContextOptions()) { }

        /// <summary>
        /// Inicializa una nueva instancia de la clase
        /// </summary>
        /// <param name="options">Opciones de construcción</param>
        public DbContext(DbContextOptions options) {
            Initialize(options);
        }

        #endregion

        #region Methods

        /// <summary>
        /// Inicializa la instancia de contexto construyendolo
        /// según las opciones de configuración
        /// </summary>
        /// <param name="options">Opciones de construcción</param>
        internal void Initialize(DbContextOptions options) {
            Options = options ?? throw Error.ArgumentException(nameof(options));
            Builder = new DbContextBuilder(options);
            TransactionInfo = new TransactionInfo(string.Empty);
        }

        #endregion

        #region Query

        /// <summary>
        /// Implementación interna de una consulta mapeando el resultado a una entidad de tipo T
        /// </summary>
        /// <typeparam name="TEntity">Tipo de la entidad en la que se mapeará el resultado</typeparam>
        /// <param name="command">Definición del comando a ejecutar</param>
        /// <returns>Una enumeración del conjunto de resultados</returns>
        private IEnumerable<TEntity> QueryImpl<TEntity>(CommandDefinition command) where TEntity : class, new() {
            Type entityType = typeof(TEntity);

            IDbConnection connection;
            if (TransactionInfo.InTransaction) {
                connection = TransactionInfo.Transaction.Connection;
            } else {
                connection = Builder.CreateConnection(true);
            }
            using (IDbCommand cmd = command.CreateCommand(connection)) {
                using (IDataReader reader = cmd.ExecuteReader()) {
                    if (reader.FieldCount == 0)
                        yield break;

                    var deserializer = DeserializerCacheStore.GetDeserializer<TEntity>(UseEntityDescriptor);
                    Type type = Nullable.GetUnderlyingType(typeof(TEntity)) ?? entityType;
                    var convertToType = Nullable.GetUnderlyingType(entityType) ?? entityType;
                    while (reader.Read()) {
                        object val = deserializer(reader);

                        if (val == null || val is TEntity) {
                            yield return (TEntity)val;
                        } else {
                            yield return (TEntity)Convert.ChangeType(val, convertToType, CultureInfo.InvariantCulture);
                        }
                    }
                    while (reader.NextResult()) { /* Ignore subsecuencia */
                    }
                    reader.Close();
                }
            }
            if (!TransactionInfo.InTransaction) {
                connection?.Close();
                connection?.Dispose();
            }
        }

        /// <summary>
        /// Ejecuta una consulta sql
        /// </summary>
        /// <typeparam name="TEntity">Tipo de la entidad resultado</typeparam>
        /// <param name="query">Consulta a ejecutar</param>
        /// <param name="parameters">Parámetros de la consulta</param>
        /// <returns>Conjunto de resultados</returns>
        public IEnumerable<TEntity> ExecuteQuery<TEntity>(string query, object parameters = null) where TEntity : class, new() {
            query = query?.Trim() ?? throw Error.ArgumentException(nameof(query));
            return QueryImpl<TEntity>(new CommandDefinition(query, this, Parameter.GetParameters(parameters), TransactionInfo.Transaction));
        }

        /// <summary>
        /// Ejecuta una consulta sql
        /// </summary>
        /// <typeparam name="TEntity">Tipo de la entidad resultado</typeparam>
        /// <param name="query">Consulta a ejecutar</param>
        /// <param name="parameters">Parámetros de la consulta</param>
        /// <returns>Conjunto de resultados</returns>
        public IEnumerable<TEntity> ExecuteQuery<TEntity>(string query, params Parameter[] parameters) where TEntity : class, new() {
            query = query?.Trim() ?? throw Error.ArgumentException(nameof(query));
            parameters = parameters ?? new Parameter[0];
            return QueryImpl<TEntity>(new CommandDefinition(query, this, parameters, TransactionInfo.Transaction));
        }

        /// <summary>
        /// Ejecuta una consulta sql
        /// </summary>
        /// <param name="query">Consulta a ejecutar</param>
        /// <param name="parameters">Parámetros de la consulta</param>
        /// <returns>Conjunto de resultados</returns>
        public IEnumerable<dynamic> ExecuteQuery(string query, object parameters = null) {
            query = query?.Trim() ?? throw Error.ArgumentException(nameof(query));
            return QueryImpl<dynamic>(new CommandDefinition(query, this, Parameter.GetParameters(parameters), TransactionInfo.Transaction));
        }

        /// <summary>
        /// Ejecuta una consulta sql
        /// </summary>
        /// <param name="query">Consulta a ejecutar</param>
        /// <param name="parameters">Parámetros de la consulta</param>
        /// <returns>Conjunto de resultados</returns>
        public IEnumerable<dynamic> ExecuteQuery(string query, params Parameter[] parameters) {
            query = query?.Trim() ?? throw Error.ArgumentException(nameof(query));
            parameters = parameters ?? new Parameter[0];
            return QueryImpl<dynamic>(new CommandDefinition(query, this, parameters, TransactionInfo.Transaction));
        }

        #endregion

        #region NonQuery

        /// <summary>
        /// Inicia una transacción
        /// </summary>
        /// <returns>Transacción iniciada</returns>
        public IDbTransaction BeginTransaction() {
            if (!TransactionInfo.InTransaction) {
                StackTrace stackTrace = new StackTrace();
                var method = stackTrace.GetFrame(1).GetMethod().Name;
                TransactionInfo = new TransactionInfo(method.ToMD5());
            }
            return BeginTransaction(IsolationLevel.ReadCommitted);
        }

        /// <summary>
        /// Inicia una transacción
        /// </summary>
        /// <param name="level">Nivel de aislamiento</param>
        /// <returns>Transacción iniciada</returns>
        public IDbTransaction BeginTransaction(IsolationLevel level) {
            IDbTransaction tran = null;
            if (!TransactionInfo.InTransaction) {
                var method = TransactionInfo.CallerId;
                if (string.IsNullOrWhiteSpace(TransactionInfo.CallerId)) {
                    StackTrace stackTrace = new StackTrace();
                    method = stackTrace.GetFrame(1).GetMethod().Name.ToMD5();
                }
                TransactionInfo = new TransactionInfo(method, Builder.CreateConnection(true).BeginTransaction(level));
            }
            return tran;
        }

        /// <summary>
        /// Confirma los cambios efectuados por la ejecución de los comandos
        /// </summary>
        public void CommitChanges() {
            if (TransactionInfo.InTransaction) {
                StackTrace stackTrace = new StackTrace();
                var method = stackTrace.GetFrame(1).GetMethod().Name;
                if (TransactionInfo.CallerId == method.ToMD5()) {
                    TransactionInfo.Transaction.Commit();
                    TransactionInfo.Transaction?.Connection?.Close();
                    TransactionInfo.Transaction?.Connection?.Dispose();
                    TransactionInfo.Reset();
                }
            }
        }

        /// <summary>
        /// Descarta los cambios efectuados por la ejecución de los comandos
        /// </summary>
        public void RollbackChanges() {
            if (TransactionInfo.InTransaction) {
                TransactionInfo.Transaction.Rollback();
                TransactionInfo.Transaction?.Connection?.Close();
                TransactionInfo.Transaction?.Connection?.Dispose();
                TransactionInfo.Reset();
            }
        }

        /// <summary>
        /// Implementación interna de la ejecución de un comando SQL
        /// </summary>
        /// <param name="command">Definición del comando a ejecutar</param>
        /// <returns>Cantidad de registros afectados</returns>
        private int NonQueryImpl(CommandDefinition command) {
            int total = 0;
            if (TransactionInfo.InTransaction) {
                using (IDbCommand cmd = command.CreateCommand(TransactionInfo.Transaction.Connection)) {
                    total = cmd.ExecuteNonQuery();
                }
            } else {
                using (IDbConnection connection = Builder.CreateConnection(true)) {
                    using (IDbCommand cmd = command.CreateCommand(connection)) {
                        total = cmd.ExecuteNonQuery();
                    }
                    connection.Close();
                }
            }

            return total;
        }

        /// <summary>
        /// Ejecuta un comando de acción como Insert, Update, Delete y obtiene el
        /// número de registros afectados
        /// </summary>
        /// <param name="command">Comando a ejecutar</param>
        /// <returns>Cantidad de registros afectados</returns>
        public int ExecuteNonQuery(IDbCommand command) {
            int total = 0;
            if (TransactionInfo.InTransaction) {
                total = command.ExecuteNonQuery();
            } else {
                using (IDbConnection connection = Builder.CreateConnection(true)) {
                    command.Connection = connection;
                    total = command.ExecuteNonQuery();
                    connection.Close();
                }
            }

            return total;
        }

        /// <summary>
        /// Ejecuta un comando de acción como Insert, Update, Delete y obtiene el
        /// número de registros afectados
        /// </summary>
        /// <param name="nonQuery">Comando a ejecutar</param>
        /// <param name="parameters">Parámetros de la consulta</param>
        /// <returns>Cantidad de registros afectados</returns>
        public int ExecuteNonQuery(string nonQuery, object parameters) {
            return ExecuteNonQuery(nonQuery, Parameter.GetParameters(parameters).ToArray());
        }

        /// <summary>
        /// Ejecuta un comando de acción como Insert, Update, Delete y obtiene el
        /// número de registros afectados
        /// </summary>
        /// <param name="nonQuery">Comando a ejecutar</param>
        /// <param name="parameters">Parámetros de la consulta</param>
        /// <returns>Cantidad de registros afectados</returns>
        public int ExecuteNonQuery(string nonQuery, params Parameter[] parameters) {
            nonQuery = nonQuery?.Trim() ?? throw Error.ArgumentException(nameof(nonQuery));
            parameters = parameters ?? new Parameter[0];
            return NonQueryImpl(new CommandDefinition(nonQuery, this, parameters, TransactionInfo.Transaction));
        }

        #endregion

        #region IDisposable

        /// <summary>
        /// Destruye la instancia
        /// </summary>
        public void Dispose() {
            if (TransactionInfo.InTransaction)
                TransactionInfo.Reset();
            TransactionInfo = null;
            Options = null;
            Builder = null;
        }

        #endregion
    }
}
