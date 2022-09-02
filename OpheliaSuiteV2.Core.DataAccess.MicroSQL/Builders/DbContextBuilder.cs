using MySql.Data.MySqlClient;
using Npgsql;
using OpheliaSuiteV2.Core.DataAccess.MicroSQL.QueryBuilder.Compilers;
using Oracle.ManagedDataAccess.Client;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;

namespace OpheliaSuiteV2.Core.DataAccess.MicroSQL.Builders {

    /// <summary>
    /// Provee funcionalidad para construir los
    /// artefactos necesarios para el funcionamiendo
    /// de un <see cref="IDbContext"/>
    /// </summary>
    public sealed class DbContextBuilder {

        #region Properties

        /// <summary>
        /// Opciones de configuración del contexto
        /// </summary>
        public DbContextOptions Options {
            get; private set;
        }

        /// <summary>
        /// Fabrica del proveedor
        /// </summary>
        public DbProviderFactory Factory {
            get; private set;
        }

        /// <summary>
        /// Compilador Sql
        /// </summary>
        internal Compiler SqlCompiler {
            get; set;
        }

        #endregion

        #region Builders

        /// <summary>
        /// Inializa una nueva instancia de la clase
        /// </summary>
        /// <param name="options">Opciones del contexto</param>
        public DbContextBuilder(DbContextOptions options) {
            Initialize(options);
        }

        #endregion

        #region Methods

        /// <summary>
        /// Inializa la instancia aplicando la lógica para construir
        /// la fabrica correspondiente a la configuración
        /// </summary>
        /// <param name="options">Opciones del contexto</param>
        private void Initialize(DbContextOptions options) {
            Options = options ?? throw Error.ArgumentException(nameof(options));
            switch (Options.DbType) {
                case DbContextType.MySql:
                    Factory = DbProviderFactories.GetFactory(new MySqlConnection(Options.ConnectionString));
                    SqlCompiler = new MySqlCompiler();
                    break;
                case DbContextType.PostgreSql:
                    Factory = DbProviderFactories.GetFactory(new NpgsqlConnection(Options.ConnectionString));
                    SqlCompiler = new PostgresCompiler();
                    break;
                case DbContextType.Oracle:
                    Factory = DbProviderFactories.GetFactory(new OracleConnection(Options.ConnectionString));
                    SqlCompiler = new OracleCompiler();
                    break;
                default: //SqlServer
                    Factory = DbProviderFactories.GetFactory(new SqlConnection(Options.ConnectionString));
                    SqlCompiler = new SqlServerCompiler();
                    break;
            }
        }

        /// <summary>
        /// Crea una conexión con la configuración
        /// asignada en las opciones
        /// </summary>
        /// <returns>Configuración</returns>
        public IDbConnection CreateConnection(bool open = false) {
            IDbConnection connection = Factory.CreateConnection();
            connection.ConnectionString = Options.ConnectionString;
            if (open)
                connection.Open();
            return connection;
        }

        #endregion
    }
}