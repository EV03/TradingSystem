namespace Domain.CashDesk
{
    public interface ICardReaderController
    {
        /// <summary>
        /// Wartet darauf, dass eine Karte gelesen wird.
        /// </summary>
        /// <param name="amount">Der Betrag der Transaktion.</param>
        /// <param name="contextData">Kontextspezifische Daten (z. B. Challenge).</param>
        /// <returns>Das Ergebnis der Kartenautorisierung.</returns>
        Task<ICardReaderResult> WaitForCardReadAsync(int amount, byte[] contextData);

        /// <summary>
        /// Bestätigt die aktuelle Transaktion.
        /// </summary>
        void Confirm(string message);

        /// <summary>
        /// Bricht die aktuelle Transaktion ab.
        /// </summary>
        void Abort(string message);
    }
    
}