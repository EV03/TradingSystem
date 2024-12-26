//------------------------------------------------------------------------------
// <auto-generated>
//     Dieser Code wurde von einem Tool generiert.
//     Laufzeitversion:4.0.30319.42000
//
//     Änderungen an dieser Datei können falsches Verhalten verursachen und gehen verloren, wenn
//     der Code erneut generiert wird.
// </auto-generated>
//------------------------------------------------------------------------------

namespace CashDesk.Infrastructure.Bank
{
    
    
    ///  <summary>
    /// </summary>
    [Tecan.Sila2.SilaFeatureAttribute(true, "banking")]
    [Tecan.Sila2.SilaIdentifierAttribute("BankServer")]
    [Tecan.Sila2.SilaDisplayNameAttribute("Bank Server")]
    public partial interface IBankServer
    {
        
        ///  <summary>
        /// </summary>
        /// <param name="amount"></param>
        /// <returns></returns>
        [return: Tecan.Sila2.SilaIdentifierAttribute("ReturnValue")]
        TransactionContext CreateContext(long amount);
        
        ///  <summary>
        /// </summary>
        /// <param name="transactionContextId"></param>
        /// <param name="account"></param>
        /// <param name="authorizationToken"></param>
        void AuthorizePayment(string transactionContextId, string account, string authorizationToken);
    }
    
    ///  <summary>
    /// </summary>
    public struct TransactionContext
    {
        
        private string _contextId;
        
        private System.IO.Stream _challenge;
        
        private long _amount;
        
        ///  <summary>
        /// Initializes a new instance
        /// </summary>
        public TransactionContext(string contextId, System.IO.Stream challenge, long amount)
        {
            _contextId = contextId;
            _challenge = challenge;
            _amount = amount;
        }
        
        ///  <summary>
        /// </summary>
        public string ContextId
        {
            get
            {
                return _contextId;
            }
        }
        
        ///  <summary>
        /// </summary>
        public System.IO.Stream Challenge
        {
            get
            {
                return _challenge;
            }
        }
        
        ///  <summary>
        /// </summary>
        [Tecan.Sila2.UnitAttribute("€", Mole=1, Factor=0.01D)]
        public long Amount
        {
            get
            {
                return _amount;
            }
        }
    }
}
