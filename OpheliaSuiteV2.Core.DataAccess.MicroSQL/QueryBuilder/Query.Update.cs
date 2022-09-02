using OpheliaSuiteV2.Core.DataAccess.MicroSQL.ORM.Internal;
using OpheliaSuiteV2.Core.DataAccess.MicroSQL.QueryBuilder.Clauses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace OpheliaSuiteV2.Core.DataAccess.MicroSQL.QueryBuilder {
    internal partial class Query {

        public Query AsUpdate(object data) {
            var dictionary = new Dictionary<string, object>();

            var props = data.GetType().GetRuntimeProperties();

            foreach (var item in props) {
                dictionary.Add(item.Name, item.GetValue(data));
            }

            return AsUpdate(dictionary);
        }

        public Query AsUpdate(IEnumerable<string> columns, IEnumerable<object> values) {

            if ((columns?.Count() ?? 0) == 0 || (values?.Count() ?? 0) == 0) {
                throw new InvalidOperationException("Columns and Values cannot be null or empty");
            }

            if (columns.Count() != values.Count()) {
                throw new InvalidOperationException("Columns count should be equal to Values count");
            }

            Method = "update";

            ClearComponent("update").AddComponent("update", new InsertClause {
                Columns = columns.ToList(),
                Values = values.ToList()
            });

            return this;
        }

        public Query AsUpdate(IReadOnlyDictionary<string, object> data) {

            if (data == null || data.Count == 0) {
                throw new InvalidOperationException("Values dictionary cannot be null or empty");
            }

            Method = "update";

            ClearComponent("update").AddComponent("update", new InsertClause {
                Columns = data.Keys.ToList(),
                Values = data.Values.ToList(),
            });

            return this;
        }

        /// <summary>
        /// Prepara el objeto de consulta como un Update
        /// </summary>
        /// <param name="properties">Propiedades a usar como columnas</param>
        /// <returns>Objeto de consulta</returns>
        public Query AsUpdate(IEnumerable<PropertySnapshot> properties) {
            if (properties == null || !properties.Any())
                throw Error.ArgumentException(nameof(properties));

            List<string> columnsList = new List<string>();
            List<object> valuesList = new List<object>();
            List<Type> typesList = new List<Type>();
            List<string> parameterNamesList = new List<string>();

            foreach (PropertySnapshot prop in properties) {
                string parName = $"p{CountParameters}";
                columnsList.Add(prop.Descriptor.ColumnName);
                typesList.Add(prop.Descriptor.Property.PropertyType);
                valuesList.Add(prop.Value);
                parameterNamesList.Add(parName);
                Parameters.Add(new Parameter(parName, prop.Value, prop.Descriptor.Property.PropertyType));
                CountParameters++;
            }

            Method = "update";

            ClearComponent("update").AddComponent("update", new InsertClause {
                Columns = columnsList,
                Values = valuesList,
                Types = typesList,
                ParameterNames = parameterNamesList
            });

            return this;
        }

    }
}