using System.IO;

namespace OpheliaSuiteV2.Core.DataAccess.MicroSQL.ORM {

    /// <summary>
    /// Define los atributos y métodos de un servicio de aplicación
    /// </summary>
    public interface IAppService {

        #region Properties

        /// <summary>
        /// Contexto de datos
        /// </summary>
        AbstractDbContext Context { get; }

        /// <summary>
        /// Obtiene el registrador de eventos
        /// </summary>
        StreamWriter Logger { get; }

        #endregion
    }
}
