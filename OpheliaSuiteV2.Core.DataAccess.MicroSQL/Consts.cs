using System;
using System.Collections.Generic;

namespace OpheliaSuiteV2.Core.DataAccess.MicroSQL {

    /// <summary>
    /// Provee constantes del sistema
    /// </summary>
    internal static class Consts {

        #region Consts

        /// <summary>
        /// Nombre por defecto de la cadena de conexión
        /// </summary>
        public const string CONNECTIONSTRING_NAME = "MicroSQL";

        #endregion

        #region Static

        /// <summary>
        /// Obtiene un valor que indica si el tipo
        /// es válido para ser una propiedad de entidad
        /// </summary>
        /// <param name="type">Tipo a validar</param>
        /// <returns>Valor que indica si es válido</returns>
        public static bool IsValidPropertyType(Type type) {
            return EntityValidTypes.Contains(type);
        }

        /// <summary>
        /// Obtiene un valor que indica si el tipo
        /// es válido para ser una propiedad de navegación de entidad
        /// </summary>
        /// <param name="type">Tipo a validar</param>
        /// <returns>Valor que indica si es válido</returns>
        public static bool IsValidNavigationPropertyType(Type type) {
            bool isValid = !IsValidPropertyType(type);
            if (isValid)
                isValid = !(type.Name == "Object" || type.IsInterface || type.IsAbstract || type.IsValueType || type.IsEnum || type.IsArray || type.IsCOMObject || type.IsGenericType || type.IsGenericTypeDefinition);

            return isValid;
        }

        /// <summary>
        /// Tipos de datos válidos para las propiedades de una entidad
        /// que mapean campos de la tabla
        /// </summary>
        private static readonly List<Type> EntityValidTypes = new List<Type>(new Type[] {
            typeof(ushort),
            typeof(UInt16),
            typeof(uint),
            typeof(UInt32),
            typeof(ulong),
            typeof(UInt64),
            typeof(SByte),
            typeof(sbyte),
            typeof(sbyte[]),
            typeof(Single),
            typeof(float),
            typeof(decimal),
            typeof(Decimal),
            typeof(double),
            typeof(Double),
            typeof(short),
            typeof(Int16),
            typeof(int),
            typeof(Int32),
            typeof(long),
            typeof(Int64),
            typeof(Guid),
            typeof(Byte),
            typeof(byte),
            typeof(byte[]),
            typeof(string),
            typeof(char),
            typeof(Char),
            typeof(String),
            typeof(DateTime),
            typeof(DateTimeOffset),
            typeof(TimeSpan),
            typeof(bool),
            typeof(Boolean),
            typeof(ushort?),
            typeof(UInt16?),
            typeof(uint?),
            typeof(UInt32?),
            typeof(ulong?),
            typeof(UInt64?),
            typeof(SByte?),
            typeof(sbyte?),
            typeof(Single?),
            typeof(float?),
            typeof(decimal?),
            typeof(Decimal?),
            typeof(double?),
            typeof(Double?),
            typeof(short?),
            typeof(Int16?),
            typeof(int?),
            typeof(Int32?),
            typeof(long?),
            typeof(Int64?),
            typeof(Guid?),
            typeof(Byte?),
            typeof(byte?),
            typeof(char?),
            typeof(Char?),
            typeof(DateTime?),
            typeof(DateTimeOffset?),
            typeof(TimeSpan?),
            typeof(bool?),
            typeof(Boolean?)
        });

        #endregion
    }
}
