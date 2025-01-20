namespace Domain.CashDesk;

public enum CashDeskAction
{
    StartNewSale,
    ProductScanned,
    FinishSale,
    PayWithCash,
    PayWithCard,
    CancelPayment,
    CompletePayment,
    DisableExpressMode,
    Complete   // Added this action to allow the CashDeskStateMachine to transition to the Idle state from any state
}