using System;
using System.Collections.Generic;

namespace OpheliaSuiteV2.Core.DataAccess.MicroSQL.Dynamic {
    internal sealed class FieldNames {
        #region Members

        private string[] _fieldNames;
        /// <summary>
        /// Obtiene el arreglo de nombres de propiedades
        /// </summary>
        public string[] Fields => this._fieldNames;

        /// <summary>
        /// Diccionario de los indices de las propiedades
        /// </summary>
        private readonly Dictionary<string, int> _fieldNamesLookup;

        /// <summary>
        /// Obtiene la cantidad de propiedades que hay
        /// </summary>
        public int FieldCount => this._fieldNames.Length;

        #endregion

        #region Builders

        /// <summary>
        /// Inicializa una nueva instancia de la clase
        /// </summary>
        /// <param name="fieldNames">Arreglo de nombres de propiedades</param>
        public FieldNames(string[] fieldNames) {
            this._fieldNames = fieldNames ?? throw new ArgumentNullException(nameof(fieldNames));

            this._fieldNamesLookup = new Dictionary<string, int>(fieldNames.Length, StringComparer.Ordinal);
            for (int i = fieldNames.Length - 1; i >= 0; i--) {
                string key = fieldNames[i];
                if (key != null)
                    this._fieldNamesLookup[key] = i;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Obtiene el indice de un nombre de propiedad
        /// </summary>
        /// <param name="name">Nombre de la propiedad</param>
        /// <returns>Indice en el que se encuentra</returns>
        public int IndexOfName(string name) {
            return (name != null && this._fieldNamesLookup.TryGetValue(name, out int result)) ? result : -1;
        }

        /// <summary>
        /// Agrega un nuevo nombre de propiedad al arreglo
        /// </summary>
        /// <param name="name">Nombre de la nueva propiedad</param>
        /// <returns>El indice que ocupa el nuevo nombre</returns>
        public int AddField(string name) {
            if (name == null)
                throw new ArgumentNullException(nameof(name));

            if (this._fieldNamesLookup.ContainsKey(name))
                throw new InvalidOperationException("El campo ya existe: " + name);
            int oldLen = this._fieldNames.Length;
            Array.Resize(ref this._fieldNames, oldLen + 1);
            this._fieldNames[oldLen] = name;
            this._fieldNamesLookup[name] = oldLen;
            return oldLen;
        }

        /// <summary>
        /// Obtiene un valor que indica si el campo existe
        /// </summary>
        /// <param name="name">Nombre del campo</param>
        /// <returns>Valor que indica si el campo existe</returns>
        public bool FieldExists(string name) => name != null && this._fieldNamesLookup.ContainsKey(name);

        #endregion
    }
}
