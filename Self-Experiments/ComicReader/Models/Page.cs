using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ComicReader.Models;
public class Page
{
    public int Id {get; set;}
    public int ChapterId {get; set;}
    public int PageNumber {get; set;}
    public string PageUrl {get; set;} = string.Empty;
    public virtual Chapter? Chapter {get; set;}
}