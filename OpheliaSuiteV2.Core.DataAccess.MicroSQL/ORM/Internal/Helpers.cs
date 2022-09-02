using OpheliaSuiteV2.Core.DataAccess.MicroSQL.Attributes;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;

namespace OpheliaSuiteV2.Core.DataAccess.MicroSQL.ORM.Internal {

    /// <summary>
    /// Provee métodos de ayuda
    /// </summary>
    internal static class Helpers {

        #region Entities

        /// <summary>
        /// Obtiene el nombre de la tabla de la entidad
        /// </summary>
        /// <param name="entity">Entidad</param>
        /// <returns>Nombre de la tabla</returns>
        public static string GetTableName(object entity) {
            if (entity == null)
                throw Error.ArgumentException(nameof(entity));
            return GetTableName(entity.GetType());
        }

        /// <summary>
        /// Obtiene el nombre de la tabla de la entidad
        /// </summary>
        /// <typeparam name="TEntity">Tipo de la entidad</typeparam>
        /// <returns>Nombre de la tabla</returns>
        public static string GetTableName<TEntity>() {
            return GetTableName(typeof(TEntity));
        }

        /// <summary>
        /// Obtiene el nombre de la tabla de la entidad
        /// </summary>
        /// <param name="entityType">Tipo de la entidad</param>
        /// <returns>Nombre de la tabla</returns>
        public static string GetTableName(Type entityType) {
            if (entityType == null)
                throw Error.ArgumentException(nameof(entityType));
            string name = entityType.Name;
            TableNameAttribute attribute = entityType.GetCustomAttribute<TableNameAttribute>();
            if (attribute != null)
                name = attribute.Name;

            return name;
        }

        /// <summary>
        /// Obtiene el nombre de la columna en la propiedad
        /// </summary>
        /// <param name="property">Propiedad</param>
        /// <returns>Nombre de la columna</returns>
        public static string GetColumnName(PropertyInfo property) {
            if (property == null)
                throw Error.ArgumentException(nameof(property));
            string name = property.Name;
            ColumnNameAttribute attribute = property.GetCustomAttribute<ColumnNameAttribute>();
            if (attribute != null)
                name = attribute.Name;

            return name;
        }

        /// <summary>
        /// Obtiene la enumeración de columnas a 
        /// partir de las propiedades de una entidad
        /// </summary>
        /// <param name="entity">Entidad</param>
        /// <returns>Columnas</returns>
        public static IEnumerable<string> GetColumns(object entity) {
            if (entity == null)
                throw Error.ArgumentException(nameof(entity));
            return GetColumns(entity.GetType());
        }

        /// <summary>
        /// Obtiene la enumeración de columnas a 
        /// partir de las propiedades de una entidad
        /// </summary>
        /// <param name="entityType">Tipo de la entidad</param>
        /// <returns>Columnas</returns>
        public static IEnumerable<string> GetColumns(Type entityType) {
            if (entityType == null)
                throw Error.ArgumentException(nameof(entityType));
            List<string> columns = new List<string>();
            foreach (PropertyInfo prop in entityType.GetProperties().Where(p => p.GetCustomAttribute<IgnoredPropertyAttribute>() == null && Consts.IsValidPropertyType(p.PropertyType))) {
                ColumnNameAttribute fieldName = prop.GetCustomAttribute<ColumnNameAttribute>();
                if (fieldName != null)
                    columns.Add(fieldName.Name);
                else
                    columns.Add(prop.Name);
            }
            return columns;
        }

        /// <summary>
        /// Obtiene la enumeración de columnas a 
        /// partir de las propiedades de una entidad
        /// </summary>
        /// <typeparam name="TEntity">Tipo de la entidad</typeparam>
        /// <returns>Columnas</returns>
        public static IEnumerable<string> GetColumns<TEntity>() {
            return GetColumns(typeof(TEntity));
        }

        /// <summary>
        /// Escribe recursivamente toda la pila de llamados
        /// </summary>
        /// <param name="ex">Excepción a escribir</param>
        /// <param name="sb">Constructor de cadena usado para escribir la excepción</param>
        public static void WriteException(Exception ex, ref StringBuilder sb) {
            if (ex == null || sb == null)
                return;

            if (sb.ToString().Length > 0)
                sb.AppendLine($"{Environment.NewLine}----");

            sb.AppendFormat(CultureInfo.InvariantCulture, $"Exception:{ex.GetType().Name}{Environment.NewLine}Message: {{0}}{Environment.NewLine}StackTrace: {{1}}", ex.Message, ex.StackTrace);

            if (ex.InnerException != null)
                WriteException(ex.InnerException, ref sb);
        }

        #endregion
    }
}
