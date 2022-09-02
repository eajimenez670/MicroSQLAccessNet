using System;

namespace OpheliaSuiteV2.Core.DataAccess.MicroSQL.Attributes {

    /// <summary>
    /// Marca las propiedades que serán ignoradas en
    /// todo el contexto de acceso a datos
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public sealed class IgnoredPropertyAttribute : Attribute {
    }
}
