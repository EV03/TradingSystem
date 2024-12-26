namespace Domain.CashDesk;

public class AuthorizationResult
{
    public bool Success { get; set; } 
    public DateTime Timestamp { get; set; }
    public string? ErrorMessage { get; set; }
    
    public AuthorizationResult(bool success, string? errorMessage)
    {
        Success = success;
        ErrorMessage = errorMessage;
        Timestamp = DateTime.Now;
    }
}