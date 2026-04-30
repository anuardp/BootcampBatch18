using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity_Framework.Models;

public class BorrowBook
{
    public int BorrowID {get; set;}
    public DateTime BorrowBookStart {get; set;}
    public DateTime BorrowBookDue {get; set;}
    public int BookID {get; set;}
    [Required]
    [MaxLength(200)]
    public int VisitorID {get; set;}
    public Book Book {get; set;}
    public Visitor Visitor {get; set;}
}