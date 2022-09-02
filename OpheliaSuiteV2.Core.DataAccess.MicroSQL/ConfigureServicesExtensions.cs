using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace OpheliaSuiteV2.Core.DataAccess.MicroSQL {
    /// <summary>
    /// Provee métodos extendidos para la
    /// inyección del servicio MicroSQL
    /// a la colección de servicios de .Net Core
    /// </summary>
    public static class ConfigureServicesExtensions {

        #region IServiceProvider

        /// <summary>
        /// Instancia al proveedor de servicios
        /// </summary>
        internal static IServiceProvider ServiceProvider {
            get; set;
        }

        #endregion

        #region IServiceCollection

        /// <summary>
        /// Inyecta un contexto de base de datos
        /// a la colección de servicios con una
        /// configuración específica
        /// </summary>
        /// <typeparam name="T">Tipo abstracto</typeparam>
        /// <typeparam name="TInstance">Tip de contexto a inyectar</typeparam>
        /// <param name="services">Collección de servicios</param>
        /// <param name="options">Opciones de configuración del contexto</param>
        /// <returns>Colección de servicios</returns>
        private static IServiceCollection AddDbContextInternal<T, TInstance>(IServiceCollection services, DbContextOptions options)
            where T : class
            where TInstance : DbContext, T, new() {
            if (services == null)
                throw Error.ArgumentException(nameof(services));
            if (options == null)
                throw Error.ArgumentException(nameof(options));

            services.AddTransient<T, TInstance>((s) => {
                TInstance instance = (TInstance)Activator.CreateInstance(typeof(TInstance));
                instance.Initialize(options);
                return instance;
            });

            return services;
        }

        /// <summary>
        /// Inyecta un contexto de base de datos
        /// a la colección de servicios con una
        /// configuración específica
        /// </summary>
        /// <typeparam name="TInstance">Tip de contexto a inyectar</typeparam>
        /// <param name="services">Collección de servicios</param>
        /// <param name="configuration">Instancia del archivo de configuración</param>
        /// <param name="paramName">Nombre del parámetro en el archivo de configuración</param>
        /// <returns>Colección de servicios</returns>
        public static IServiceCollection AddDbContext<TInstance>(this IServiceCollection services, IConfiguration configuration, string paramName = Consts.CONNECTIONSTRING_NAME) where TInstance : DbContext, IDbContext, new() {
            return AddDbContext<IDbContext, TInstance>(services, configuration, paramName);
        }

        /// <summary>
        /// Inyecta un contexto de base de datos
        /// a la colección de servicios con una
        /// configuración específica
        /// </summary>
        /// <typeparam name="T">Tipo abstracto</typeparam>
        /// <typeparam name="TInstance">Tip de contexto a inyectar</typeparam>
        /// <param name="services">Collección de servicios</param>
        /// <param name="configuration">Instancia del archivo de configuración</param>
        /// <param name="paramName">Nombre del parámetro en el archivo de configuración</param>
        /// <returns>Colección de servicios</returns>
        public static IServiceCollection AddDbContext<T, TInstance>(this IServiceCollection services, IConfiguration configuration, string paramName = Consts.CONNECTIONSTRING_NAME)
            where T : class
            where TInstance : DbContext, T, new() {

            paramName = paramName?.Trim() ?? Consts.CONNECTIONSTRING_NAME;
            if (configuration == null)
                throw Error.ArgumentException(nameof(configuration));
            IConfigurationSection section = configuration.GetSection(paramName);
            if (!section.Exists() || section.Value != null)
                throw Error.ArgumentException(nameof(paramName));

            DbContextOptions options = new DbContextOptions();

            IEnumerable<IConfigurationSection> sections = section.GetChildren();
            foreach (IConfigurationSection sec in sections) {
                if (sec.Exists() && !string.IsNullOrWhiteSpace(sec.Value)) {
                    PropertyInfo prop = options.GetType().GetProperty(sec.Key);
                    if (prop != null) {
                        if (prop.CanWrite) {
                            if (prop.PropertyType.IsEnum) {
                                prop.SetValue(options, Enum.Parse(prop.PropertyType, sec.Value.ToString()));
                            } else {
                                prop.SetValue(options, sec.Value);
                            }
                        }
                    }
                }
            }

            return AddDbContext<T, TInstance>(services, options);
        }

        /// <summary>
        /// Inyecta un contexto de base de datos
        /// a la colección de servicios con una
        /// configuración específica
        /// </summary>
        /// <typeparam name="TInstance">Tip de contexto a inyectar</typeparam>
        /// <param name="services">Collección de servicios</param>
        /// <param name="options">Opciones de configuración del contexto</param>
        /// <returns>Colección de servicios</returns>
        public static IServiceCollection AddDbContext<TInstance>(this IServiceCollection services, DbContextOptions options) where TInstance : DbContext, IDbContext, new() {
            return AddDbContext<IDbContext, TInstance>(services, options);
        }

        /// <summary>
        /// Inyecta un contexto de base de datos
        /// a la colección de servicios con una
        /// configuración específica
        /// </summary>
        /// <typeparam name="T">Tipo abstracto</typeparam>
        /// <typeparam name="TInstance">Tip de contexto a inyectar</typeparam>
        /// <param name="services">Collección de servicios</param>
        /// <param name="options">Opciones de configuración del contexto</param>
        /// <returns>Colección de servicios</returns>
        public static IServiceCollection AddDbContext<T, TInstance>(this IServiceCollection services, DbContextOptions options) where T : class
            where TInstance : DbContext, T, new() {
            return AddDbContextInternal<T, TInstance>(services, options);
        }

        #endregion
    }
}
