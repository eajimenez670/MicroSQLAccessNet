using OpheliaSuiteV2.Core.DataAccess.MicroSQL.Builders;
using System;
using System.Collections.Generic;
using System.Data;

namespace OpheliaSuiteV2.Core.DataAccess.MicroSQL {

    /// <summary>
    /// Define las propiedades y métodos de
    /// un contexto de base de datos
    /// </summary>
    public interface IDbContext : IDisposable {

        #region Properties

        /// <summary>
        /// Opciones de configuración del contexto
        /// </summary>
        DbContextOptions Options {
            get;
        }

        /// <summary>
        /// Informa ción de la transacción en curso
        /// </summary>
        TransactionInfo TransactionInfo {
            get;
        }

        /// <summary>
        /// Constructor de artefactos del contexto
        /// </summary>
        DbContextBuilder Builder {
            get;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Inicia una transacción
        /// </summary>
        /// <returns>Transacción iniciada</returns>
        IDbTransaction BeginTransaction();
        /// <summary>
        /// Inicia una transacción
        /// </summary>
        /// <param name="level">Nivel de aislamiento</param>
        /// <returns>Transacción iniciada</returns>
        IDbTransaction BeginTransaction(IsolationLevel level);

        /// <summary>
        /// Confirma los cambios efectuados por la ejecución de los comandos
        /// </summary>
        void CommitChanges();

        /// <summary>
        /// Descarta los cambios efectuados por la ejecución de los comandos
        /// </summary>
        void RollbackChanges();

        /// <summary>
        /// Ejecuta una consulta sql
        /// </summary>
        /// <typeparam name="TEntity">Tipo de la entidad resultado</typeparam>
        /// <param name="query">Consulta a ejecutar</param>
        /// <param name="parameters">Parámetros de la consulta</param>
        /// <returns>Conjunto de resultados</returns>
        IEnumerable<TEntity> ExecuteQuery<TEntity>(string query, object parameters = null) where TEntity : class, new();

        /// <summary>
        /// Ejecuta una consulta sql
        /// </summary>
        /// <typeparam name="TEntity">Tipo de la entidad resultado</typeparam>
        /// <param name="query">Consulta a ejecutar</param>
        /// <param name="parameters">Parámetros de la consulta</param>
        /// <returns>Conjunto de resultados</returns>
        IEnumerable<TEntity> ExecuteQuery<TEntity>(string query, params Parameter[] parameters) where TEntity : class, new();

        /// <summary>
        /// Ejecuta una consulta sql
        /// </summary>
        /// <param name="query">Consulta a ejecutar</param>
        /// <param name="parameters">Parámetros de la consulta</param>
        /// <returns>Conjunto de resultados</returns>
        IEnumerable<dynamic> ExecuteQuery(string query, object parameters = null);

        /// <summary>
        /// Ejecuta una consulta sql
        /// </summary>
        /// <param name="query">Consulta a ejecutar</param>
        /// <param name="parameters">Parámetros de la consulta</param>
        /// <returns>Conjunto de resultados</returns>
        IEnumerable<dynamic> ExecuteQuery(string query, params Parameter[] parameters);

        /// <summary>
        /// Ejecuta un comando de acción como Insert, Update, Delete y obtiene el
        /// número de registros afectados
        /// </summary>
        /// <param name="command">Comando a ejecutar</param>
        /// <returns>Cantidad de registros afectados</returns>
        int ExecuteNonQuery(IDbCommand command);

        /// <summary>
        /// Ejecuta un comando de acción como Insert, Update, Delete y obtiene el
        /// número de registros afectados
        /// </summary>
        /// <param name="nonQuery">Comando a ejecutar</param>
        /// <param name="parameters">Parámetros de la consulta</param>
        /// <returns>Cantidad de registros afectados</returns>
        int ExecuteNonQuery(string nonQuery, object parameters);

        /// <summary>
        /// Ejecuta un comando de acción como Insert, Update, Delete y obtiene el
        /// número de registros afectados
        /// </summary>
        /// <param name="nonQuery">Comando a ejecutar</param>
        /// <param name="parameters">Parámetros de la consulta</param>
        /// <returns>Cantidad de registros afectados</returns>
        int ExecuteNonQuery(string nonQuery, params Parameter[] parameters);

        #endregion
    }
}
