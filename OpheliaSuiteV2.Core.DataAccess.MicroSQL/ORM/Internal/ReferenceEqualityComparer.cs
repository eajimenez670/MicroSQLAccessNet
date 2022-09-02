using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace OpheliaSuiteV2.Core.DataAccess.MicroSQL.ORM.Internal {

    /// <summary>
    /// Comparador de refecrencias
    /// </summary>
    public sealed class ReferenceEqualityComparer : IEqualityComparer<object> {

        #region Properties

        /// <summary>
        /// Obtiene la unica instancia de la clase
        /// </summary>
        public static ReferenceEqualityComparer Instance { get; } = new ReferenceEqualityComparer();

        #endregion

        #region Builders

        /// <summary>
        /// Inicializa una nueva instancia de la clase
        /// </summary>
        private ReferenceEqualityComparer() {
        }

        #endregion

        #region MyRegion

        bool IEqualityComparer<object>.Equals(object x, object y) => ReferenceEquals(x, y);

        int IEqualityComparer<object>.GetHashCode(object obj) => RuntimeHelpers.GetHashCode(obj);

        #endregion
    }
}
