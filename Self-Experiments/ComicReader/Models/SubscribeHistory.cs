using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ComicReader.Models;
public class SubscribeHistory
{
    public int Id {get; set;}
    public int CustomerId {get; set;}
    public DateTime StartDate {get; set;}
    public DateTime EndDate {get; set;}
    public DateTime TransactionDate {get; set;}
    public string? PaymentMethod {get; set;}

    public virtual Customer? Customer {get; set;}
}