using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ComicReader.Models;
public class Comic
{
    public int Id {get; set;}
    public string Title {get; set;} = string.Empty;
    public string Publisher {get; set;} = string.Empty;
    public string Author {get; set;} = string.Empty;
    public string Genre {get; set;} = string.Empty;
    public DateTime YearReleased {get; set;}
    public bool IsOnGoing {get; set;} = true;
    public bool IsPremium {get; set;}
    public DateTime DateAdded {get; set;}
    public int TotalChapter {get; set;}

    public virtual ICollection<Chapter>? Chapters {get; set;} = new List<Chapter>();
}