using System;

namespace OpheliaSuiteV2.Core.DataAccess.MicroSQL.ORM {

    /// <summary>
    /// Define los atributos y métodos de un servicio de dominio
    /// </summary>
    public interface IDomainService : IDisposable {

        #region Methods

        /// <summary>
        /// Asigna el contexto de datos a todos los repositorios del servicio
        /// </summary>
        /// <param name="context">Contexto de datos</param>
        void SetContext(AbstractDbContext context);

        #endregion
    }
}
