using System;

namespace OpheliaSuiteV2.Core.DataAccess.MicroSQL {

    /// <summary>
    /// Encapsula la configuración por defecto
    /// </summary>
    public static class DbDefaultConfig {

        #region Config

        /// <summary>
        /// Tiempo de espera antes para la ejecución de comandos sql
        /// </summary>
        public static int CommandTimeout = 60;
        /// <summary>
        /// Proveedor de servicios
        /// </summary>
        public static IServiceProvider ServiceProvider { get; set; }

        #endregion
    }
}
