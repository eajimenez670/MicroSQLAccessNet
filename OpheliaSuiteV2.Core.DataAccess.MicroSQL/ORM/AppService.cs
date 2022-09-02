using OpheliaSuiteV2.Core.DataAccess.MicroSQL.ORM.Internal;

using System;
using System.IO;
using System.Text;

namespace OpheliaSuiteV2.Core.DataAccess.MicroSQL.ORM {

    /// <summary>
    /// Clase base de los servicios de aplicación
    /// </summary>
    public abstract class AppService : IAppService {
        #region Properties

        /// <summary>
        /// Contexto de datos
        /// </summary>
        public AbstractDbContext Context { get; private set; }
        /// <summary>
        /// Obtiene el registrador de eventos
        /// </summary>
        public StreamWriter Logger { get; private set; }

        #endregion

        #region Builders

        /// <summary>
        /// Inicializa una nueva instancia de la clase
        /// </summary>
        /// <param name="context">Contexto de datos</param>
        /// <param name="logger">Regitrador de eventos</param>
        public AppService(AbstractDbContext context, StreamWriter logger) {
            Context = context ?? throw Error.ArgumentException(nameof(context));
            Logger = logger ?? throw Error.ArgumentException(nameof(logger));
        }

        #endregion

        #region Methods

        /// <summary>
        /// Da manejo a un excepción
        /// </summary>
        /// <param name="ex">Excepción a manejar</param>
        /// <param name="result">Resultado</param>
        protected virtual TResult HandleException<TResult>(Exception ex, TResult result) {
            StringBuilder builder = new StringBuilder();
            Helpers.WriteException(ex, ref builder);
            Logger.WriteLine(builder.ToString());

            return result;
        }

        #endregion
    }
}
