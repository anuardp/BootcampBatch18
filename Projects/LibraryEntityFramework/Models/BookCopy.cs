using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity_Framework.Models;

public class BookCopy
{
    
    public int BookCopyID{get; set;}
    
    public int BookID {get; set;}
    public virtual Book? Book{get; set;}
    
    [Required]
    [MaxLength(100)]
    public string? BookCode {get; set;}
    public bool IsAvailable{get; set;}

    public virtual ICollection<BorrowBook>? BorrowBooks{get; set;}
    
}