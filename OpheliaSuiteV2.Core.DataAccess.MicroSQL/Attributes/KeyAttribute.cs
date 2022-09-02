using System;

namespace OpheliaSuiteV2.Core.DataAccess.MicroSQL.Attributes {

    /// <summary>
    /// Marca la propiedad principal de la llave
    /// primaria de la entidad
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public sealed class KeyAttribute : Attribute {

        #region Properties

        /// <summary>
        /// Tipo de llave primaria
        /// </summary>
        public KeyType Type {
            get; private set;
        }

        #endregion

        #region Builders

        /// <summary>
        /// Inicializa una nueva instancia de la clase
        /// </summary>
        /// <param name="type">Tipo de llave primaria</param>
        public KeyAttribute(KeyType type = KeyType.Manually) {
            Type = type;
        }

        #endregion
    }

    /// <summary>
    /// Tipo de llave
    /// </summary>
    public enum KeyType {
        /// <summary>
        /// Llave manual
        /// </summary>
        Manually,
        /// <summary>
        /// Llave autoincrementada calculada
        /// a partir de las partes auxiliares.
        /// Normalmente usada en llaves compuestas.
        /// El tipo de dato de la propiedad
        /// debe ser <see cref="Int16"/>, <see cref="Int32"/>, <see cref="Int64"/>, <see cref="short"/>,
        /// <see cref="int"/>, <see cref="long"/>
        /// </summary>
        Autoincrement,
        /// <summary>
        /// Identificador autoincrementado por la
        /// base de datos. El tipo de dato de la propiedad
        /// debe ser <see cref="Int16"/>, <see cref="Int32"/>, <see cref="Int64"/>, <see cref="short"/>,
        /// <see cref="int"/>, <see cref="long"/>
        /// </summary>
        Identity,
        /// <summary>
        /// Identificador único calculado por la base de datos.
        /// El tipo de dato de la propiedad debe ser <see cref="Guid"/>
        /// o <see cref="string"/>
        /// </summary>
        Uniqueidentifier
    }
}
