using Domain.CashDesk;
using Stateless;

namespace CashDesk.Application;

public enum CashDeskSaleState
{
    Init,
    Idle,
    SaleActive,
    PreparePayment,
    PaymentInProgress,
    PrintingReceipt
}

public class CashDeskSalesStateMachine
{
    private readonly StateMachine<CashDeskSaleState, CashDeskAction> _stateMachine;

    private readonly StateMachine<CashDeskSaleState, CashDeskAction>.TriggerWithParameters<string>
        _productScannedTrigger;

    private readonly CashDeskExpressModeStateMachine _expressModeStateMachine;
    private readonly ICashBoxController _cashBoxController;
    private readonly IPrinterController _printerController;
    private readonly IDisplayController _displayController;
    private readonly IBarcodeScannerController _barcodeScannerController;
    private readonly ICardReaderController _cardReaderController;
    private readonly ISaleService _saleService;
    private readonly IPaymentService _paymentService;
    private readonly IExpressModeService _expressModeService;
    private readonly ITransactionRepository _transactionRepository;
    private List<SaleItem> _saleItem;
    private string _paymentMethod;

    public CashDeskSalesStateMachine(ICashBoxController cashBoxController, IPrinterController printerController,
        IBarcodeScannerController barcodeScannerController, ICardReaderController cardReaderController,
        CashDeskExpressModeStateMachine expressModeStateMachine,
        IDisplayController displayController, ISaleService saleService, IPaymentService paymentService,
        IExpressModeService expressModeService, ITransactionRepository transactionRepository)
    {
        _cashBoxController = cashBoxController;
        _printerController = printerController;
        _barcodeScannerController = barcodeScannerController;
        _displayController = displayController;
        _cardReaderController = cardReaderController;
        _saleService = saleService;
        _paymentService = paymentService;
        _expressModeService = expressModeService;
        _transactionRepository = transactionRepository;
        _paymentMethod = string.Empty;
        _saleItem = new List<SaleItem>();

        _stateMachine = new StateMachine<CashDeskSaleState, CashDeskAction>(CashDeskSaleState.Init);
        _expressModeStateMachine = expressModeStateMachine;
        _productScannedTrigger = _stateMachine.SetTriggerParameters<string>(CashDeskAction.ProductScanned);
        ConfigureStateMachine();
        Console.WriteLine("CashDesk initialized.");
        _stateMachine.Activate();
        _stateMachine.Fire(CashDeskAction.StartNewSale); // go to idle state to call onEntry
    }

    private void ConfigureStateMachine()
    {
        ConfigureInitState();
        ConfigureIdleState();
        ConfigureSaleActiveState();
        ConfigurePreparePaymentState();
        ConfigurePaymentInProgressState();
        ConfigurePrintingReceiptState();
    }

    private void ConfigureInitState()
    {
        _stateMachine.Configure(CashDeskSaleState.Init)
            .Permit(CashDeskAction.StartNewSale, CashDeskSaleState.Idle);
    }

    private void ConfigureIdleState()
    {
        _stateMachine.Configure(CashDeskSaleState.Idle)
            .Permit(CashDeskAction.StartNewSale, CashDeskSaleState.SaleActive)
            .PermitReentry(CashDeskAction.DisableExpressMode)
            .OnEntryFrom(CashDeskAction.DisableExpressMode, DisableExpressMode)
            .OnEntryFrom(CashDeskAction.StartNewSale, DisplayIdleState)
            .OnEntryFrom(CashDeskAction.Complete, DisplayIdleState);
    }

    private void ConfigureSaleActiveState()
    {
        _stateMachine.Configure(CashDeskSaleState.SaleActive)
            .PermitReentry(_productScannedTrigger.Trigger)
            .PermitIf(CashDeskAction.FinishSale, CashDeskSaleState.PreparePayment, () => !_saleService.Sale.IsEmpty())
            .OnEntryFrom(CashDeskAction.StartNewSale, StartSale)
            .OnEntryFrom(_productScannedTrigger, ProcessScannedProduct);
    }

    private void ConfigurePreparePaymentState()
    {
        _stateMachine.Configure(CashDeskSaleState.PreparePayment)
            .PermitIf(CashDeskAction.PayWithCard, CashDeskSaleState.PaymentInProgress,
                () => _expressModeStateMachine.State() == CashDeskExpressModeState.Disabled)
            .Permit(CashDeskAction.PayWithCash, CashDeskSaleState.PaymentInProgress)
            .Permit(CashDeskAction.CancelPayment, CashDeskSaleState.SaleActive)
            .OnEntryFrom(CashDeskAction.FinishSale, () =>
            {
                DisplayText("Choose payment method");
                _barcodeScannerController.StopListeningToBarcodes();
            })
            .OnEntryFrom(CashDeskAction.CancelPayment, () => _cashBoxController.StartListeningToCashbox())
            .OnExit(() => _cashBoxController.StopListeningToCashbox());
    }

    private void ConfigurePaymentInProgressState()
    {
        _stateMachine.Configure(CashDeskSaleState.PaymentInProgress)
            .Permit(CashDeskAction.CompletePayment, CashDeskSaleState.PrintingReceipt)
            .Permit(CashDeskAction.CancelPayment, CashDeskSaleState.PreparePayment)
            .OnEntryFrom(CashDeskAction.PayWithCard, async () => await ProcessCardPayment())
            .OnEntryFrom(CashDeskAction.PayWithCash, async () => await ProcessCashPayment());
    }

    private void ConfigurePrintingReceiptState()
    {
        _stateMachine.Configure(CashDeskSaleState.PrintingReceipt)
            .Permit(CashDeskAction.Complete, CashDeskSaleState.Idle)
            .OnEntry(async () => await FinalizeSale());
    }

    private void DisableExpressMode()
    {
        _expressModeStateMachine.Fire(CashDeskExpressModeActions.DisableExpressMode);
        DisplayText("Waiting for Sale to start. Express Mode Disabled");
    }

    private void DisplayIdleState()
    {
        var expressModeState = _expressModeStateMachine.State() == CashDeskExpressModeState.Enabled
            ? "Enabled"
            : "Disabled";
        DisplayText($"Waiting for Sale to start. Express Mode {expressModeState}");
        Console.WriteLine($"Press 'Start New Sale' to begin a new sale. expressmode state: {expressModeState}");
        _cashBoxController.StartListeningToCashbox();
    }

    private void StartSale()
    {
        Console.WriteLine("Sale started, scan products");
        DisplayText("Sale started, scan products");
        _barcodeScannerController.StartListeningToBarcodes();
        _saleService.StartSale();
    }

    private void ProcessScannedProduct(string barcode)
    {
        _barcodeScannerController.StopListeningToBarcodes();

        try
        {
            if (_expressModeStateMachine.State() == CashDeskExpressModeState.Enabled &&
                _saleService.Sale.Items.Count >= 2)
            {
                DisplayText("Cannot add more than 2 different products in Express Mode.");
            }
            else
            {
                var task = _saleService.AddProductToSale(barcode);
                DisplayText($"{barcode}, {(double)task.Result.Price / 100}€");
                Console.WriteLine($"Product with barcode {barcode} added to sale.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Failed to add product with barcode {barcode} to sale, Reason: {ex.Message}");
        }
        finally
        {
            _barcodeScannerController.StartListeningToBarcodes();
        }
    }

    private async Task ProcessCardPayment()
    {
        Console.WriteLine("card payment started, waiting for card swipe");
        DisplayText("Card payment, please swipe card");
        try
        {
            await _paymentService.PayCardAsync(_saleService.GetSaleTotal());
            Console.WriteLine("Updating data...");
            await Task.Delay(2000); // simulate card payment or cancel time
            _cardReaderController.Confirm("");
            _paymentMethod = "CardPayment";
            _stateMachine.Fire(CashDeskAction.CompletePayment);
        }
        catch (Exception)
        {
            Console.WriteLine("Card payment got canceled, choose card or cash payment again");
            _stateMachine.Fire(CashDeskAction.CancelPayment);
        }
    }

    private async Task ProcessCashPayment()
    {
        Console.WriteLine("cash payment started");
        DisplayText("cash payment");
        await _paymentService.PayCashAsync(_saleService.GetSaleTotal());
        _paymentMethod = "CashPayment";
        _stateMachine.Fire(CashDeskAction.CompletePayment);
    }

    private async Task FinalizeSale()
    {
        Console.WriteLine("payment successful");
        await _saleService.FinishSaleAsync();
        await Task.Delay(1000);
        Console.WriteLine("Printing receipt...\n");
        DisplayText("Printing receipt...");
        await Task.Delay(3000); // simulating printing time
        _printerController.Print("Receipt");
        Console.WriteLine("Receipt printed. Returning to Idle state...\n\n");

        if (_saleService.Sale?.Items != null) _saleItem = _saleService.Sale.Items;
        _transactionRepository.SaveTransaction(new Transaction(_saleItem, _paymentMethod));
        _saleItem = new List<SaleItem>();
        _paymentMethod = string.Empty;

        if (_expressModeStateMachine.State() == CashDeskExpressModeState.Disabled)
        {
            var expressMode = _expressModeService.IsExpressMode();
            if (expressMode) _expressModeStateMachine.Fire(CashDeskExpressModeActions.EnableExpressMode);
        }

        _stateMachine.Fire(CashDeskAction.Complete);
    }

    private void DisplayText(string text)
    {
        _displayController.DisplayText(text);
    }

    public void Fire(CashDeskAction action)
    {
        if (_stateMachine.CanFire(action)) _stateMachine.Fire(action);
        else
        {
            if (_saleService.Sale.IsEmpty() && action == CashDeskAction.FinishSale)
            {
                Console.WriteLine("Sale is empty, cannot finish sale");
            }

            if (_expressModeStateMachine.State() == CashDeskExpressModeState.Enabled &&
                action == CashDeskAction.PayWithCard)
            {
                Console.WriteLine("Card Payment not allowed in Express Mode, choose cash payment!");
            }
            else
                Console.WriteLine($"Action {action} cannot be fired in state {_stateMachine.State}");
        }
    }

    public void Fire(CashDeskAction action, string barcode)
    {
        if (_stateMachine.CanFire(action)) _stateMachine.Fire(_productScannedTrigger, barcode);
        else Console.WriteLine($"Action {action} cannot be fired in state {_stateMachine.State}");
    }

    public bool CanFire(CashDeskAction action)
    {
        return _stateMachine.CanFire(action) && action != CashDeskAction.CancelPayment;
    }

    public enum CashDeskExpressModeState
    {
        Disabled,
        Enabled
    }

    public enum CashDeskExpressModeActions
    {
        EnableExpressMode,
        DisableExpressMode
    }

    public class CashDeskExpressModeStateMachine
    {
        private readonly StateMachine<CashDeskExpressModeState, CashDeskExpressModeActions> _stateMachine;
        private readonly IDisplayController _displayController;

        public CashDeskExpressModeStateMachine(IDisplayController displayController)
        {
            _displayController = displayController;
            _stateMachine =
                new StateMachine<CashDeskExpressModeState, CashDeskExpressModeActions>(
                    CashDeskExpressModeState.Disabled);
            ConfigureStateMachine();
        }

        private void ConfigureStateMachine()
        {
            _stateMachine.Configure(CashDeskExpressModeState.Disabled)
                .Permit(CashDeskExpressModeActions.EnableExpressMode, CashDeskExpressModeState.Enabled)
                .OnEntryFrom(CashDeskExpressModeActions.DisableExpressMode,
                    () => Console.WriteLine("Express Mode disabled"));

            _stateMachine.Configure(CashDeskExpressModeState.Enabled)
                .Permit(CashDeskExpressModeActions.DisableExpressMode, CashDeskExpressModeState.Disabled)
                .OnEntry(() => Console.WriteLine("Express Mode Enabled"));
        }

        public void Fire(CashDeskExpressModeActions action)
        {
            if (_stateMachine.CanFire(action)) _stateMachine.Fire(action);
            else Console.WriteLine($"Action {action} cannot be fired in state {_stateMachine.State}");
        }

        public bool CanFire(CashDeskExpressModeActions action)
        {
            return _stateMachine.CanFire(action);
        }

        public CashDeskExpressModeState State()
        {
            return _stateMachine.State;
        }
    }
}