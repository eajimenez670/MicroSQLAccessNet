using System;
using System.Collections.Generic;
using System.Data;

namespace OpheliaSuiteV2.Core.DataAccess.MicroSQL {

    /// <summary>
    /// Mapeador de tipos
    /// </summary>
    internal static class MapperTypes {

        #region Meppers

        /// <summary>
        /// Mapa de tipos .Net a ADO
        /// </summary>
        public static Dictionary<Type, DbType> TypeMap = new Dictionary<Type, DbType> {
            [typeof(byte)] = DbType.Byte,
            [typeof(Byte)] = DbType.Byte,
            [typeof(sbyte)] = DbType.SByte,
            [typeof(SByte)] = DbType.SByte,
            [typeof(short)] = DbType.Int16,
            [typeof(ushort)] = DbType.UInt16,
            [typeof(int)] = DbType.Int32,
            [typeof(Int32)] = DbType.Int32,
            [typeof(uint)] = DbType.UInt32,
            [typeof(UInt32)] = DbType.UInt32,
            [typeof(long)] = DbType.Int64,
            [typeof(ulong)] = DbType.UInt64,
            [typeof(float)] = DbType.Single,
            [typeof(double)] = DbType.Double,
            [typeof(Double)] = DbType.Double,
            [typeof(decimal)] = DbType.Decimal,
            [typeof(Decimal)] = DbType.Decimal,
            [typeof(bool)] = DbType.Boolean,
            [typeof(Boolean)] = DbType.Boolean,
            [typeof(string)] = DbType.AnsiString,
            [typeof(String)] = DbType.AnsiString,
            [typeof(char)] = DbType.AnsiStringFixedLength,
            [typeof(Char)] = DbType.AnsiStringFixedLength,
            [typeof(Guid)] = DbType.Guid,
            [typeof(DateTime)] = DbType.DateTime,
            [typeof(DateTimeOffset)] = DbType.DateTimeOffset,
            [typeof(TimeSpan)] = DbType.Time,
            [typeof(byte[])] = DbType.Binary,
            [typeof(Byte[])] = DbType.Binary,
            [typeof(byte?)] = DbType.Byte,
            [typeof(Byte?)] = DbType.Byte,
            [typeof(sbyte?)] = DbType.SByte,
            [typeof(SByte?)] = DbType.SByte,
            [typeof(short?)] = DbType.Int16,
            [typeof(ushort?)] = DbType.UInt16,
            [typeof(int?)] = DbType.Int32,
            [typeof(uint?)] = DbType.UInt32,
            [typeof(UInt32?)] = DbType.UInt32,
            [typeof(long?)] = DbType.Int64,
            [typeof(ulong?)] = DbType.UInt64,
            [typeof(float?)] = DbType.Single,
            [typeof(double?)] = DbType.Double,
            [typeof(Double?)] = DbType.Double,
            [typeof(decimal?)] = DbType.Decimal,
            [typeof(Decimal?)] = DbType.Decimal,
            [typeof(bool?)] = DbType.Boolean,
            [typeof(Boolean?)] = DbType.Boolean,
            [typeof(char?)] = DbType.AnsiStringFixedLength,
            [typeof(Char?)] = DbType.AnsiStringFixedLength,
            [typeof(Guid?)] = DbType.Guid,
            [typeof(DateTime?)] = DbType.DateTime,
            [typeof(DateTimeOffset?)] = DbType.DateTimeOffset,
            [typeof(TimeSpan?)] = DbType.Time,
            [typeof(object)] = DbType.Object,
            [typeof(Object)] = DbType.Object
        };

        #endregion
    }
}
