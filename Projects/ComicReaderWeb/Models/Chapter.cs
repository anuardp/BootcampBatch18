using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ComicReader.Models;
public class Chapter
{
    public int Id {get; set;}
    public int ComicId {get; set;}
    public int ChapterNumber {get; set;}
    public int TotalPage {get; set;}
    public DateTime? DateUploaded {get; set;}
    public virtual Comic? Comic {get; set;}
    public virtual ICollection<Page>? Pages {get; set;}
}