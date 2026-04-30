using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity_Framework.Models;

public class Visitor
{
    public int VisitorID {get; set;}

    [Required]
    [MaxLength(150)]
    public string Name {get; set;} = string.Empty;
    [Required]
    [EmailAddress]
    public string Email {get; set;} = string.Empty;
    
    [Required]
    [Phone]
    public string PhoneNumber {get; set;} = string.Empty;
    
    [Required]
    public DateTime BirthDate {get; set;}

    public bool IsLibraryMember {get; set;} = false;
    public virtual ICollection<BorrowBook>? BorrowBooks{get; set;}
}