using System.Collections.Generic;

namespace OpheliaSuiteV2.Core.DataAccess.MicroSQL.ORM.Internal {

    /// <summary>
    /// Provee métodos extendidos internos
    /// </summary>
    internal static class InternalExtensions {

        #region PropertySnapshot

        /// <summary>
        /// Convierte una enumeración de propiedades
        /// a una de parámetros
        /// </summary>
        /// <param name="props">Propiedades</param>
        /// <returns>Parámetros</returns>
        public static IEnumerable<Parameter> ToParameters(this IEnumerable<PropertySnapshot> props) {
            List<Parameter> parameters = new List<Parameter>();
            if (props != null) {
                foreach (PropertySnapshot prop in props) {
                    parameters.Add(new Parameter($"p{prop.Descriptor.Name}", prop.Value, prop.Descriptor.Property.PropertyType));
                }
            }
            return parameters;
        }

        #endregion
    }
}
