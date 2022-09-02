using OpheliaSuiteV2.Core.DataAccess.MicroSQL.Exceptions;
using System;

namespace OpheliaSuiteV2.Core.DataAccess.MicroSQL {
    /// <summary>
    /// Encapsula las opciones de configuración
    /// para un contexto de bases de datos
    /// </summary>
    public sealed class DbContextOptions {

        #region Properties

        /// <summary>
        /// Tipo de conextto de base de datos
        /// </summary>
        public DbContextType DbType {
            get; internal set;
        }

        /// <summary>
        /// Cadena de conexión a la base de datos
        /// </summary>
        public string ConnectionString {
            get; set;
        }

        /// <summary>
        /// Tiempo de espera antes para la ejecución de comandos sql
        /// </summary>
        public int CommandTimeout {
            get; set;
        }

        #endregion

        #region Builders

        /// <summary>
        /// Inicializa una nueva instancia de la clase
        /// </summary>
        internal DbContextOptions() {
            DbType = DbContextType.SqlServer;
            ConnectionString = string.Empty;
            CommandTimeout = DbDefaultConfig.CommandTimeout;
        }

        #endregion

        #region Factories

        /// <summary>
        /// Crea una nueva instancia de <see cref="DbContextOptions"/>
        /// </summary>
        /// <param name="dbType">Tipo de contexto</param>
        /// <param name="connectionString">Cadena de conexión</param>
        /// <returns>Instancia de <see cref="DbContextOptions"/></returns>
        public static DbContextOptions Create(string dbType, string connectionString) {
            if (Enum.TryParse(dbType.ToString(), out DbContextType _dbtype)) {
                return Create(_dbtype, connectionString);
            }
            throw new InvalidDbContextTypeException(dbType);
        }

        /// <summary>
        /// Crea una nueva instancia de <see cref="DbContextOptions"/>
        /// </summary>
        /// <param name="dbType">Tipo de contexto</param>
        /// <param name="connectionString">Cadena de conexión</param>
        /// <returns>Instancia de <see cref="DbContextOptions"/></returns>
        public static DbContextOptions Create(int dbType, string connectionString) {
            if (Enum.TryParse(dbType.ToString(), out DbContextType _dbtype)) {
                return Create(_dbtype, connectionString);
            }
            throw new InvalidDbContextTypeException(dbType.ToString());
        }

        /// <summary>
        /// Crea una nueva instancia de <see cref="DbContextOptions"/>
        /// </summary>
        /// <param name="dbType">Tipo de contexto</param>
        /// <param name="connectionString">Cadena de conexión</param>
        /// <returns>Instancia de <see cref="DbContextOptions"/></returns>
        public static DbContextOptions Create(DbContextType dbType, string connectionString) {
            if (string.IsNullOrWhiteSpace(connectionString))
                throw Error.ArgumentException(nameof(connectionString));

            return new DbContextOptions() { DbType = dbType, ConnectionString = connectionString.Trim() };
        }

        #endregion
    }
}
