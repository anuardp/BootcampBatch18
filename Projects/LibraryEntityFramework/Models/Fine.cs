using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity_Framework.Models;

public class Fine
{
    public int FineID {get; set;}
    public int BorrowID {get; set;}
    public virtual BorrowBook? BorrowBook{get; set;}
    public decimal TotalFine {get; set;}
    public bool HasPayFine {get; set;}
    public DateTime? PaidDate {get; set;}

}
