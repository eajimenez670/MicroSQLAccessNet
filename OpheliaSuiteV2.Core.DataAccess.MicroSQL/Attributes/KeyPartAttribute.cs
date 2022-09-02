using System;

namespace OpheliaSuiteV2.Core.DataAccess.MicroSQL.Attributes {

    /// <summary>
    /// Marca la propiedad como parte de la llave
    /// primaria de la entidad, cuando la llave es compuesta
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public sealed class KeyPartAttribute : Attribute {
    }
}
