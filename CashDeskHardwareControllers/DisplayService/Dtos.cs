//------------------------------------------------------------------------------
// <auto-generated>
//     Dieser Code wurde von einem Tool generiert.
//     Laufzeitversion:4.0.30319.42000
//
//     Änderungen an dieser Datei können falsches Verhalten verursachen und gehen verloren, wenn
//     der Code erneut generiert wird.
// </auto-generated>
//------------------------------------------------------------------------------

namespace CashDeskHardwareControllers.DisplayService
{
    using Tecan.Sila2;
    
    
    ///  <summary>
    /// Data transfer object for the request of the Set Display Text command
    /// </summary>
    [ProtoBuf.ProtoContractAttribute()]
    public class SetDisplayTextRequestDto : Tecan.Sila2.ISilaTransferObject, Tecan.Sila2.ISilaRequestObject
    {
        
        private Tecan.Sila2.StringDto _displayText;
        
        ///  <summary>
        /// Create a new instance
        /// </summary>
        public SetDisplayTextRequestDto()
        {
        }
        
        ///  <summary>
        /// Create a new instance
        /// </summary>
        /// <param name="store">An object to organize binaries.</param>
        ///  <param name="displayText"></param>
        public SetDisplayTextRequestDto(string displayText, Tecan.Sila2.IBinaryStore store)
        {
            DisplayText = new Tecan.Sila2.StringDto(displayText, store);
        }
        
        ///  <summary>
        /// </summary>
        [ProtoBuf.ProtoMemberAttribute(1)]
        public Tecan.Sila2.StringDto DisplayText
        {
            get
            {
                return _displayText;
            }
            set
            {
                _displayText = value;
            }
        }
        
        ///  <summary>
        /// Gets the command identifier for this command
        /// </summary>
        /// <returns>The fully qualified command identifier</returns>
        public string CommandIdentifier
        {
            get
            {
                return "cocome.terminal/contracts/DisplayController/v1/Command/SetDisplayText";
            }
        }
        
        ///  <summary>
        /// Validates the contents of this transfer object
        /// </summary>
        /// <returns>A validation error or null, if no validation error occurred.</returns>
        public string GetValidationErrors()
        {
            return null;
        }
    }
}
