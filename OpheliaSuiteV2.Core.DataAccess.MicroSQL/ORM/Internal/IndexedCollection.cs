using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace OpheliaSuiteV2.Core.DataAccess.MicroSQL.ORM.Internal {

    /// <summary>
    /// Provee métodos para indexar una colección de datos tipo T
    /// y realizar búsquedas más rapidas
    /// </summary>
    /// <typeparam name="T">Tipo de dato de la colección</typeparam>
    internal sealed class IndexedCollection<T> {

        #region Fields

        /// <summary>
        /// Lista de datos no indexados
        /// </summary>
        internal IList<T> NonIndexedList;
        /// <summary>
        /// Lista de datos indexados
        /// </summary>
        internal readonly IDictionary<string, ILookup<object, T>> IndexedList;
        /// <summary>
        /// Indices
        /// </summary>
        internal readonly IList<Expression<Func<T, object>>> Indexes;

        #endregion

        #region Builders

        /// <summary>
        /// Inicializa una nueva instancia de la clase
        /// </summary>
        /// <param name="source">Fuente de datos</param>
        /// <param name="indexes">Indices usados para organizar los datos</param>
        public IndexedCollection(IEnumerable<T> source, params Expression<Func<T, object>>[] indexes) {
            NonIndexedList = new List<T>(source);
            IndexedList = new Dictionary<string, ILookup<object, T>>();
            Indexes = new List<Expression<Func<T, object>>>();
            BuildIndexes(indexes);
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Busca un valor sobre una propiedad
        /// </summary>
        /// <param name="property">Expresión de la propiedad sobre la que se realiza la busqueda</param>
        /// <param name="value">Valor a buscar</param>
        /// <returns>Conjunto de resultados</returns>
        public IndexedResult FindValue(Expression<Func<T, object>> property, object value) {
            return new IndexedResult(this, new List<T>()).And(property, value);
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Construye los indices
        /// </summary>
        /// <param name="indexes">Indices como expresiones</param>
        private void BuildIndexes(Expression<Func<T, object>>[] indexes) {
            for (int i = 0; i < indexes.Length; i++) {
                string indexName = Convert.ToBase64String(Encoding.UTF8.GetBytes(PropertyName(indexes[i])));
                if (IndexedList.ContainsKey(indexName)) {
                    continue;
                }

                Indexes.Add(indexes[i]);
                IndexedList.Add(indexName, NonIndexedList.ToLookup(indexes[i].Compile()));
            }
            NonIndexedList = NonIndexedList.Except(IndexedList.SelectMany(x => x.Value).SelectMany(r => r)).ToList();
        }

        /// <summary>
        /// Obtiene el nombre de la propiedad en la expresión
        /// </summary>
        /// <param name="expression">Expresión a evaluar</param>
        /// <returns>Nombre de la propiedad en base64</returns>
        internal string PropertyName(Expression<Func<T, object>> expression) {
            if (!(expression.Body is MemberExpression body)) {
                body = ((UnaryExpression)expression.Body).Operand as MemberExpression;
            }

            return body.Member.Name;
        }

        #endregion

        #region IEnumerable

        /// <summary>
        /// Obtiene una lista de la colección
        /// </summary>
        /// <returns>Lista de tipo T</returns>
        public IList<T> ToList() {
            List<T> res = new List<T>(NonIndexedList);
            res.AddRange(IndexedList.SelectMany(x => x.Value).SelectMany(r => r));

            return res;
        }

        #endregion

        #region Classes

        /// <summary>
        /// Encapsula un conjunto de resultados de la busqueda sobre una colección indexada
        /// </summary>
        public class IndexedResult {
            #region Fields

            /// <summary>
            /// Instancia a la colección de datos
            /// </summary>
            private readonly IndexedCollection<T> _indexedCollection;
            /// <summary>
            /// Conjunto de resultados
            /// </summary>
            private readonly IEnumerable<T> _resultSet;

            #endregion

            #region Builders

            /// <summary>
            /// Inicializa una nueva instancia de la clase
            /// </summary>
            /// <param name="indexedCollection">Instancia a la colección de datos</param>
            /// <param name="resultSet">Conjunto de resultados</param>
            internal IndexedResult(IndexedCollection<T> indexedCollection, IEnumerable<T> resultSet) {
                _indexedCollection = indexedCollection;
                _resultSet = resultSet;
            }

            #endregion

            #region Methods

            /// <summary>
            /// Retorna un subconjunto de resultados donde los elementos cumplan cualquiera de los dos criterios
            /// </summary>
            /// <param name="property">Expresión de la propiedad sobre la que se realiza la busqueda</param>
            /// <param name="value">Valor a buscar</param>
            /// <returns>Subconjunto de resultados</returns>
            public IndexedResult Or(Expression<Func<T, object>> property, object value) {
                string indexName = Convert.ToBase64String(Encoding.UTF8.GetBytes(_indexedCollection.PropertyName(property)));
                if (_indexedCollection.IndexedList.ContainsKey(indexName)) {
                    return new IndexedResult(_indexedCollection, (_resultSet.Count() == 0 ? (_indexedCollection.IndexedList[indexName].Contains(value) ? _indexedCollection.IndexedList[indexName][value] : new T[0]) : (_indexedCollection.IndexedList[indexName].Contains(value) ? _resultSet.Union(_indexedCollection.IndexedList[indexName][value]) : _resultSet)));
                }

                var c = property.Compile();
                return new IndexedResult(_indexedCollection, _resultSet.Except(_indexedCollection.NonIndexedList.Where(x => c(x).Equals(value))));
            }

            /// <summary>
            /// Retorna un subconjunto de resultados donde los elementos cumplan con ambos criterios
            /// </summary>
            /// <param name="property">Expresión de la propiedad sobre la que se realiza la busqueda</param>
            /// <param name="value">Valor a buscar</param>
            /// <returns>Subconjunto de resultados</returns>
            public IndexedResult And(Expression<Func<T, object>> property, object value) {
                string indexName = Convert.ToBase64String(Encoding.UTF8.GetBytes(_indexedCollection.PropertyName(property)));
                if (_indexedCollection.IndexedList.ContainsKey(indexName)) {
                    return new IndexedResult(_indexedCollection, (_resultSet.Count() == 0 ? (_indexedCollection.IndexedList[indexName].Contains(value) ? _indexedCollection.IndexedList[indexName][value] : new T[0]) : (_indexedCollection.IndexedList[indexName].Contains(value) ? _resultSet.Intersect(_indexedCollection.IndexedList[indexName][value]) : _resultSet)));
                }

                var c = property.Compile();
                return new IndexedResult(_indexedCollection, _resultSet.Intersect(_indexedCollection.NonIndexedList.Where(x => c(x).Equals(value))));
            }

            #endregion

            #region IEnumerable

            /// <summary>
            /// Obtiene una lista de la colección
            /// </summary>
            /// <returns>Lista de tipo T</returns>
            public IList<T> ToList() {
                return _resultSet.ToList();
            }

            #endregion
        }

        #endregion
    }
}
