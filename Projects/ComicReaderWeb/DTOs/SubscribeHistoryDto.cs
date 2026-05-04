namespace ComicReader.DTOs;

public class SubscribeHistoryDto()
{
    public int Id {get; set;}
    public int CustomerId { get; set; }
    public string CustomerName { get; set; } = string.Empty; 
    public string CustomerEmail { get; set; } = string.Empty;
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public DateTime TransactionDate { get; set; }
    public string? PaymentMethod { get; set; }
    
    public bool IsActive => EndDate > DateTime.UtcNow;
    public int DurationDays => (EndDate - StartDate).Days;
}