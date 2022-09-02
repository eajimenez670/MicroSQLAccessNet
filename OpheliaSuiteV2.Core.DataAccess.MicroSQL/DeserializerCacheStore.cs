using OpheliaSuiteV2.Core.DataAccess.MicroSQL.Attributes;
using OpheliaSuiteV2.Core.DataAccess.MicroSQL.Dynamic;
using OpheliaSuiteV2.Core.DataAccess.MicroSQL.ORM.Internal;

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Reflection;

namespace OpheliaSuiteV2.Core.DataAccess.MicroSQL {

    /// <summary>
    /// Almacena los serializadores de tipos
    /// </summary>
    internal static class DeserializerCacheStore {

        #region Members

        /// <summary>
        /// Caché de deserializadores por tipo
        /// </summary>
        private static readonly ConcurrentDictionary<Type, Func<IDataRecord, object>> _cache = new ConcurrentDictionary<Type, Func<IDataRecord, object>>();

        #endregion

        #region Methods

        /// <summary>
        /// Obtiene la función deserializadora para el tipo T
        /// </summary>
        /// <typeparam name="T">Tipo a deserializar</typeparam>
        /// <param name="useEntityDescriptor">Valor que indica si se usa el descriptor de entidad
        /// <returns>Función deserializadora</returns>
        public static Func<IDataRecord, object> GetDeserializer<T>(bool useEntityDescriptor = true) where T : class, new() {
            Type type = Nullable.GetUnderlyingType(typeof(T)) ?? typeof(T);
            if (type == typeof(object) || type == typeof(DynamicRow)) {
                if (!_cache.TryGetValue(type, out Func<IDataRecord, object> des)) {
                    des = CreateDynamicRowDeserializer();
                    _cache.TryAdd(type, des);
                }
                return des;
            }

            if (!_cache.TryGetValue(type, out Func<IDataRecord, object> des1)) {
                if (useEntityDescriptor)
                    EntityDescriptorMapper.GetOrAddDescriptor(type);
                des1 = CreateGenericDeserializer<T>(useEntityDescriptor);
                _cache.TryAdd(type, des1);
            }
            return des1;
        }

        /// <summary>
        /// Crea un deserializador para un tipo object cualquiera
        /// </summary>
        /// <typeparam name="T">Tipo del objeto a deserializar</typeparam>
        /// <param name="useEntityDescriptor">Valor que indica si se usa el descriptor de entidad
        /// o se realiza la serialización de forma clásica</param>
        /// <returns>Función deserializadora</returns>
        private static Func<IDataRecord, T> CreateGenericDeserializer<T>(bool useEntityDescriptor = true) where T : class, new() {
            return r => {
                T result = new T();
                Type type = typeof(T);
                for (int i = 0; i < r.FieldCount; i++) {
                    if (useEntityDescriptor) {
                        EntityDescriptor descriptor = EntityDescriptorMapper.GetOrAddDescriptor(type);
                        //Obtenemos la propiedad por el nombre y que no esté marcada para ignorar
                        PropertyDescriptor prop = descriptor.GetPropertyByColumnName(r.GetName(i));
                        if (prop != null) {
                            Type propType = Nullable.GetUnderlyingType(prop.Property.PropertyType) ?? prop.Property.PropertyType;

                            object val = r.GetValue(i);

                            if (val is DBNull)
                                val = null;

                            prop.Property.SetValue(result, (val == null ? null : Convert.ChangeType(val, propType, CultureInfo.InvariantCulture)));
                        }
                    } else {
                        //Obtenemos la propiedad por el nombre y que no esté marcada para ignorar
                        PropertyInfo prop = type.GetProperties().FirstOrDefault(p => {
                            if (p.GetCustomAttribute<IgnoredPropertyAttribute>() != null)
                                return false;
                            if (!Consts.IsValidPropertyType(p.PropertyType))
                                return false;

                            ColumnNameAttribute columnName = p.GetCustomAttribute<ColumnNameAttribute>();
                            if (columnName == null)
                                return false;
                            if (columnName.Name != r.GetName(i))
                                return false;

                            return true;
                        });
                        if (prop == null)
                            prop = type.GetProperties().FirstOrDefault(p => p.Name == r.GetName(i) && p.GetCustomAttribute<IgnoredPropertyAttribute>() == null && Consts.IsValidPropertyType(p.PropertyType));

                        if (prop != null) {
                            Type propType = Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType;

                            object val = r.GetValue(i);

                            if (val is DBNull)
                                val = null;

                            prop.SetValue(result, (val == null ? null : Convert.ChangeType(val, propType, CultureInfo.InvariantCulture)));
                        }
                    }
                }

                return result;
            };
        }

        /// <summary>
        /// Crea un deserializador para el tipo DynamicRow
        /// </summary>
        /// <returns>Función deserializadora</returns>
        private static Func<IDataRecord, object> CreateDynamicRowDeserializer() {
            return r => {
                FieldNames table = null;
                int effectiveFieldCount = r.FieldCount;
                if (table == null) {
                    string[] names = new string[effectiveFieldCount];
                    for (int i = 0; i < effectiveFieldCount; i++) {
                        names[i] = r.GetName(i);
                    }
                    table = new FieldNames(names);
                }

                var values = new object[effectiveFieldCount];

                for (int i = 0; i < values.Length; i++) {
                    object val = r.GetValue(i);
                    values[i] = val is DBNull ? null : val;
                }
                return new DynamicRow(table, values);
            };
        }

        #endregion
    }
}
