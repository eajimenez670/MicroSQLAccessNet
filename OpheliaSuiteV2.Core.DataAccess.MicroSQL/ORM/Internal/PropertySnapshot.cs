using OpheliaSuiteV2.Core.DataAccess.MicroSQL.Attributes;
using System;
using System.Globalization;
using System.Reflection;
using System.Linq;

namespace OpheliaSuiteV2.Core.DataAccess.MicroSQL.ORM.Internal {

    /// <summary>
    /// Captura el valor de una propiedade de una entidad
    /// en un momento dado
    /// </summary>
    internal sealed class PropertySnapshot {

        #region Properties

        /// <summary>
        /// Descriptor de la propiedad
        /// </summary>
        public PropertyDescriptor Descriptor { get; private set; }

        /// <summary>
        /// Valor de la propiedad
        /// </summary>
        public object Value {
            get; set;
        }

        /// <summary>
        /// Huella única que identifica el valor
        /// de la propiedad
        /// </summary>
        public string FootPrint {
            get; private set;
        }

        #endregion

        #region Builders

        /// <summary>
        /// Inicializa una nueva instancia de la clase
        /// </summary>
        /// <param name="descriptor">Información de la propiedad</param>
        /// <param name="instance">Instancia del objeto</param>
        public PropertySnapshot(PropertyDescriptor descriptor, object instance) {
            Descriptor = descriptor ?? throw Error.ArgumentException(nameof(descriptor));
            Value = (instance == null ? null : descriptor.Property.GetValue(instance));
            SetFootPrint();
        }

        #endregion

        #region Methods

        /// <summary>
        /// Asigna la huella única
        /// </summary>
        private void SetFootPrint() {
            FootPrint = GetObjectFootPrint(Value).ToMD5();
        }

        /// <summary>
        /// Obtiene la huella de un objeto
        /// </summary>
        /// <param name="obj">Objeto a obtener</param>
        /// <returns>Huella del objeto</returns>
        private string GetObjectFootPrint(object obj) {
            if (obj == null)
                return "NULL";

            string footPrint = obj.ToString();
            switch (obj.GetType().Name) {
                case "Char":
                case "String":
                case "Byte":
                case "Int16":
                case "Int32":
                case "Int64":
                case "Decimal":
                case "Double":
                case "Single":
                case "Guid":
                    break;
                case "DateTime":
                    footPrint += ((DateTime)obj).ToString("yyyyMMddHHmmssff", CultureInfo.InvariantCulture);
                    break;
                case "Char[]":
                case "String[]":
                case "Byte[]":
                case "Int16[]":
                case "Int32[]":
                case "Int64[]":
                case "Decimal[]":
                case "Double[]":
                case "Single[]":
                case "Guid[]":
                    footPrint += string.Join(",", ((System.Collections.IEnumerable)obj).OfType<object>().Select(d => d.ToString()));
                    //while (enm.MoveNext())
                    //    footPrint += ("," + enm.Current.ToString());
                    break;
                case "ICollection`1":
                case "IEnumerable`1":
                case "IEnumerable":
                case "IList`1":
                case "IReadOnlyCollection`1":
                case "IReadOnlyList`1":
                case "ICollection":
                case "IList":
                    var enm1 = ((System.Collections.IEnumerable)obj).GetEnumerator();
                    while (enm1.MoveNext())
                        footPrint += GetObjectFootPrint(enm1.Current);
                    break;
                default:
                    foreach (PropertyInfo prop in obj.GetType().GetProperties()) {
                        if (prop.CanRead) {
                            switch (prop.PropertyType.Name) {
                                case "Char":
                                case "String":
                                case "Byte":
                                case "Int16":
                                case "Int32":
                                case "Int64":
                                case "Decimal":
                                case "Double":
                                case "Single":
                                case "Guid":
                                    footPrint += prop.GetValue(obj).ToString();
                                    break;
                                case "DateTime":
                                    footPrint += ((DateTime)prop.GetValue(obj)).ToString("yyyyMMddHHmmssff", CultureInfo.InvariantCulture);
                                    break;
                                case "Char[]":
                                case "String[]":
                                case "Byte[]":
                                case "Int16[]":
                                case "Int32[]":
                                case "Int64[]":
                                case "Decimal[]":
                                case "Double[]":
                                case "Single[]":
                                case "Guid[]":
                                    var enm2 = ((System.Collections.IEnumerable)prop.GetValue(obj)).GetEnumerator();
                                    while (enm2.MoveNext())
                                        footPrint += ("," + enm2.Current.ToString());
                                    break;
                                case "ICollection`1":
                                case "IEnumerable`1":
                                case "IEnumerable":
                                case "IList`1":
                                case "IReadOnlyCollection`1":
                                case "IReadOnlyList`1":
                                case "ICollection":
                                case "IList":
                                    var enm3 = ((System.Collections.IEnumerable)prop.GetValue(obj)).GetEnumerator();
                                    while (enm3.MoveNext())
                                        footPrint += GetObjectFootPrint(enm3.Current);
                                    break;
                                default:
                                    footPrint += GetObjectFootPrint(prop.GetValue(obj));
                                    break;
                            }
                        }
                    }
                    break;
            }

            return footPrint;
        }

        #endregion
    }
}
