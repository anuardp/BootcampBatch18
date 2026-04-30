using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity_Framework.Models;

public class Book
{
    public int BookID {get; set;}

    [Required]
    [MaxLength(100)]
    public string BookISBN {get; set;} = string.Empty;
    [Required]
    [MaxLength(200)]
    public string BookTitle {get; set;} = string.Empty;
    [Required]
    [MaxLength(150)]
    public string BookPublisher {get; set;} = string.Empty;    
    public string BookAuthors {get; set;} = string.Empty;
    public string Genre {get; set;} = string.Empty;
    public int YearReleased {get; set;}


    public virtual ICollection<BookCopy>? BookCopies{get; set;}
}