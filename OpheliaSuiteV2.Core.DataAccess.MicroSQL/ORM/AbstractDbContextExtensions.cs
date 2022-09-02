using Microsoft.Extensions.DependencyInjection;

namespace OpheliaSuiteV2.Core.DataAccess.MicroSQL.ORM {

    /// <summary>
    /// Provee metodos extendidos para <see cref="AbstractDbContext"/>
    /// </summary>
    public static class AbstractDbContextExtensions {

        #region AbstractDbContext

        /// <summary>
        /// Obtiene un repositorio
        /// </summary>
        /// <typeparam name="TRepository">Tipo del repositorio a obtener</typeparam>
        /// <param name="context">Contexto quien extiende el método</param>
        /// <returns>Repositorio</returns>
        public static TRepository GetRepository<TRepository>(this AbstractDbContext context) where TRepository : IRepository {
            if (DbDefaultConfig.ServiceProvider == null)
                return default;

            TRepository rep = DbDefaultConfig.ServiceProvider.GetService<TRepository>();

            if (rep != null)
                rep.Initialize(context);

            return rep;
        }

        /// <summary>
        /// Obtiene un servicio de dominio
        /// </summary>
        /// <typeparam name="TDomainService">Tipo del servicio a obtener</typeparam>
        /// <param name="context">Contexto quien extiende el método</param>
        /// <returns>Servicio</returns>
        public static TDomainService GetDomainService<TDomainService>(this AbstractDbContext context) where TDomainService : IDomainService {
            if (DbDefaultConfig.ServiceProvider == null)
                return default;

            TDomainService rep = DbDefaultConfig.ServiceProvider.GetService<TDomainService>();

            rep.SetContext(context);

            return rep;
        }

        #endregion
    }
}
