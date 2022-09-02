using OpheliaSuiteV2.Core.DataAccess.MicroSQL.ORM;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;

namespace OpheliaSuiteV2.Core.DataAccess.MicroSQL {

    /// <summary>
    /// Encapsula las características de un comando ODK
    /// </summary>
    internal class CommandDefinition {

        #region Properties

        /// <summary>
        /// Expresión regular para buscar parámetros en una sentencia sql
        /// </summary>
        public static readonly Regex ParamsRegex = new Regex("{[a-zA-Z_]+[a-zA-Z0-9_]*}", RegexOptions.Multiline | RegexOptions.Compiled | RegexOptions.CultureInvariant);

        /// <summary>
        /// Comando sql
        /// </summary>
        public string CommandText {
            get; set;
        }

        /// <summary>
        /// Enumeración de parámetros
        /// </summary>
        public IEnumerable<Parameter> Parameters {
            get; private set;
        }

        /// <summary>
        /// Transacción asociada al comando
        /// </summary>
        public IDbTransaction Transaction {
            get; private set;
        }

        /// <summary>
        /// Tipo de comando
        /// </summary>
        public CommandType CommandType {
            get; set;
        } = CommandType.Text;

        /// <summary>
        /// Entrada de la entidad fuente del comando
        /// </summary>
        public EntityEntry Entry { get; private set; }

        /// <summary>
        /// Contexto al que pertenece el comando
        /// </summary>
        public IDbContext DbContext {
            get; private set;
        }

        /// <summary>
        /// Comando Sql
        /// </summary>
        public IDbCommand Command {
            get; private set;
        }

        #endregion

        #region Builders

        /// <summary>
        /// Inicializa una nueva instancia de la clase
        /// </summary>
        /// <param name="commandText">Comando Sql</param>
        /// <param name="dbContext">Contexto de base de datos</param>
        /// <param name="parameters">Parámetros en el comando</param>
        /// <param name="transaction">Transacción asociada</param>
        /// <param name="executeCreateCommand">Valor que indica si se ejecuta la creación de comando</param>
        /// <param name="entry">Entrada de entidad fuente del comando</param>
        public CommandDefinition(string commandText, IDbContext dbContext, IEnumerable<Parameter> parameters, IDbTransaction transaction, bool executeCreateCommand, EntityEntry entry) {
            CommandText = commandText?.Trim() ?? throw Error.ArgumentException(nameof(commandText));
            DbContext = dbContext ?? throw Error.ArgumentException(nameof(dbContext));
            Parameters = parameters;
            Transaction = transaction;
            Entry = entry;
            if (Transaction != null && executeCreateCommand)
                Command = CreateCommand(transaction.Connection);
        }

        /// <summary>
        /// Inicializa una nueva instancia de la clase
        /// </summary>
        /// <param name="commandText">Comando Sql</param>
        /// <param name="dbContext">Contexto de base de datos</param>
        /// <param name="parameters">Parámetros en el comando</param>
        /// <param name="transaction">Transacción asociada</param>
        /// <param name="executeCreateCommand">Valor que indica si se ejecuta la creación de comando</param>
        /// <param name="entry">Entrada de entidad fuente del comando</param>
        public CommandDefinition(string commandText, IDbContext dbContext, object parameters, IDbTransaction transaction, bool executeCreateCommand = false, EntityEntry entry = null) : this(commandText, dbContext, Parameter.GetParameters(parameters), transaction, executeCreateCommand, entry) { }

        #endregion

        #region Methods

        /// <summary>
        /// Crea el comando a partir de la definición y el tipo de proveedor sobre la conexión dada
        /// </summary>
        /// <param name="connection">Conexión sobre la que se creará el comando</param>
        /// <returns>Comando creado</returns>
        public IDbCommand CreateCommand(IDbConnection connection) {
            IDbCommand cmd = connection.CreateCommand();
            if (Transaction != null)
                cmd.Transaction = Transaction;

            cmd.CommandText = CommandText;
            cmd.CommandTimeout = DbContext.Options.CommandTimeout;
            cmd.CommandType = CommandType;

            return TranslateParams(cmd, Parameters, DbContext);
        }

        /// <summary>
        /// Traduce los parámetros de la forma {ParamName} a la correspondiente según el tipo de proveedor 
        /// </summary>
        /// <param name="cmd">Comando objetivo</param>
        /// <param name="parameters">Enumeración de parámetros</param>
        /// <param name="dbContext">Contexto</param>
        /// <returns>Comando resultado</returns>
        public static IDbCommand TranslateParams(IDbCommand cmd, IEnumerable<Parameter> parameters, IDbContext dbContext) {
            if (parameters == null || string.IsNullOrWhiteSpace(cmd.CommandText))
                return cmd;

            //Obtenemos el caracter usado como prefijo de parámetros para el contexto actual
            string prefix = dbContext.Builder.SqlCompiler.parameterPlaceholder;

            //Obtenemos todos los parámetros usados en el comando sql
            foreach (Match p in ParamsRegex.Matches(cmd.CommandText)) {
                //Obtenemos el nombre del parámetro
                string paramName = p.Value.Replace("{", "").Replace("}", "").Replace("[", "").Replace("]", "");

                //Validamos que el parámetro esté definido en el arreglo de parámetros
                var param = parameters.FirstOrDefault(par => par.Name == paramName);
                if (param == null)
                    throw Error.UndefinedParameterException(paramName);

                //Asignamos el prefijo al nombre del parámetro
                paramName = $"{prefix}{paramName}";

                //Validamos que no se haya agregado ya el parámetro al arreglo de parámetros del comando
                if (cmd.Parameters.IndexOf(paramName) < 0) {
                    //Creamos el parámetro
                    IDbDataParameter par = cmd.CreateParameter();
                    //Asignamos las propiedades
                    par.Direction = ParameterDirection.Input;
                    par.DbType = MapperTypes.TypeMap[(Nullable.GetUnderlyingType(param.ClrType) ?? param.ClrType)];
                    //Si el contexto es de sql server o el parámetro acepta marcarlo como que permite nulo, lo hacemos
                    if (Nullable.GetUnderlyingType(param.ClrType) != null) {
                        PropertyInfo prop = par.GetType().GetProperty("IsNullable");
                        if (prop != null && prop.CanWrite && prop.GetSetMethod() != null) {
                            prop.SetValue(par, true);
                        }
                    }
                    par.ParameterName = paramName;
                    par.Value = param.Value ?? DBNull.Value;
                    //Reemplazamos el nombre del parámetro por el formato de nombres para el contexto actual
                    string parNameReplace = (prefix == "?" ? "?" : par.ParameterName);
                    cmd.CommandText = Regex.Replace(cmd.CommandText, $"{{{param.Name}}}", $"{parNameReplace}", RegexOptions.Multiline | RegexOptions.Compiled | RegexOptions.CultureInvariant);
                    cmd.Parameters.Add(par);
                }
            }

            return cmd;
        }

        #endregion
    }
}
