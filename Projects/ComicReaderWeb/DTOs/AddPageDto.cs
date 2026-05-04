namespace ComicReader.DTOs;
public class AddPageDto
{
    public int ChapterId { get; set; }
    public int PageNumber { get; set; }
    public string PageUrl { get; set; } = string.Empty;
}