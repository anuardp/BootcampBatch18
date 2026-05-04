namespace ComicReader.DTOs;
public class SubscribeToPremiumDto
{
    public int CustomerId { get; set; }
    public int DurationDays { get; set; } = 30;
    public string? PaymentMethod { get; set; } = "QRIS";
}