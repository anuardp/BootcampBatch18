namespace ComicReader.DTOs;
public class ReadComicDto
{
    public int ComicId { get; set; }
    public string Title { get; set; } = string.Empty;
    public bool IsPremium { get; set; }
    public int SelectedChapterId { get; set; }
    public List<ChapterInfo> Chapters { get; set; } = new();
    public List<PageInfo> Pages { get; set; } = new();
}

public class ChapterInfo
{
    public int Id { get; set; }
    public int ChapterNumber { get; set; }
    public int TotalPage { get; set; }
    public DateTime DateUploaded { get; set; }
}
public class PageInfo
{
    public int Id { get; set; }
    public int PageNumber { get; set; }
    public string PageUrl { get; set; } = string.Empty;
}