using System.Data;

namespace OpheliaSuiteV2.Core.DataAccess.MicroSQL {

    /// <summary>
    /// Encapsula la información del inicio de una transacción
    /// </summary>
    public sealed class TransactionInfo {

        #region Properties

        /// <summary>
        /// Id del invocador de la tranzacción
        /// </summary>
        public string CallerId {
            get; private set;
        }

        /// <summary>
        /// Transacción
        /// </summary>
        public IDbTransaction Transaction {
            get; private set;
        }

        /// <summary>
        /// Valor que indica si existe una transacción en curso
        /// </summary>
        public bool InTransaction {
            get {
                return (Transaction != null);
            }
        }

        #endregion

        #region Builders

        /// <summary>
        /// Inicializa una nueva instancia de la clase
        /// </summary>
        /// <param name="callerId">Id del invocador de la tranzacción</param>
        /// <param name="transaction">Transacción</param>
        public TransactionInfo(string callerId, IDbTransaction transaction = null) {
            CallerId = callerId;
            Transaction = transaction;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Reinicia la configuración de la transacción
        /// </summary>
        public void Reset() {
            if (InTransaction) {
                Transaction.Dispose();
                Transaction = null;
                CallerId = string.Empty;
            }
        }

        #endregion
    }
}
