using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;

namespace OpheliaSuiteV2.Core.DataAccess.MicroSQL {

    /// <summary>
    /// Encapsula los datos de un parámetro
    /// </summary>
    public sealed class Parameter {

        #region Fields

        /// <summary>
        /// Nombre del parámetro
        /// </summary>
        private string _name;
        /// <summary>
        /// Valor del parámetro
        /// </summary>
        private object _value;

        #endregion

        #region Properties

        /// <summary>
        /// Nombre o identificador del parámetro
        /// </summary>
        public string Name {
            get {
                return _name;
            }
            set {
                if (string.IsNullOrWhiteSpace(value))
                    throw Error.ArgumentException(nameof(Name));
                _name = value.Trim();
            }
        }

        /// <summary>
        /// Tipo de dato en el contexto
        /// </summary>
        public DbType DbType {
            get; internal set;
        }

        /// <summary>
        /// Tipo de dato .Net
        /// </summary>
        public Type ClrType {
            get; internal set;
        }

        /// <summary>
        /// Valor del parámetro
        /// </summary>
        public object Value {
            get {
                return _value;
            }
            set {
                _value = value;
                CalcTypes();
            }
        }

        #endregion

        #region Builders

        /// <summary>
        /// Inicializa una nueva instancia de la clase
        /// </summary>
        /// <param name="name">Nombre del parámetro</param>
        /// <param name="value">Valor del parámetro</param>
        /// <param name="clrType">Tipo del parámetro</param>
        public Parameter(string name, object value, Type clrType) {
            if (string.IsNullOrWhiteSpace(name))
                throw Error.ArgumentException(nameof(name));
            if (clrType == null)
                throw Error.ArgumentException(nameof(clrType));
            Name = name.Trim();
            _value = value;
            ClrType = clrType;
            CalcTypes();
        }

        #endregion

        #region Methods

        /// <summary>
        /// Calcula las propiedades tipos para el valor asignado
        /// </summary>
        private void CalcTypes() {
            DbType = MapperTypes.TypeMap[ClrType];
        }

        #endregion

        #region Factories

        /// <summary>
        /// Obtiene una enumeración de parámetros a partir de las
        /// propiedades de un objeto
        /// </summary>
        /// <param name="parameters">Objeto de parámetros</param>
        /// <returns>Enumeración de parámetros</returns>
        public static IEnumerable<Parameter> GetParameters(object parameters) {
            if (parameters == null)
                return null;
            if (typeof(IEnumerable<Parameter>).IsAssignableFrom(parameters.GetType()))
                return (IEnumerable<Parameter>)parameters;
            List<Parameter> paramResult = new List<Parameter>();
            if (parameters != null) {
                foreach (PropertyInfo prop in parameters.GetType().GetProperties()) {
                    if (prop.CanRead) {
                        paramResult.Add(new Parameter(prop.Name, prop.GetValue(parameters), prop.PropertyType));
                    }
                }
            }

            return paramResult;
        }

        #endregion
    }
}
