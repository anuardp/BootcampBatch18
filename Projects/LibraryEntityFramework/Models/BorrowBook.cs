using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity_Framework.Models;

public class BorrowBook
{
    public int Id {get; set;}
    public DateTime BorrowBookStart {get; set;}
    public DateTime BorrowBookDue {get; set;}
    public DateTime? ReturnedDate {get; set;}
    public int BookCopyId {get; set;}
    public int VisitorId {get; set;}
    public virtual BookCopy? BookCopy {get; set;}
    public virtual Visitor? Visitor {get; set;}
    public virtual Fine? Fine { get; set; }
}