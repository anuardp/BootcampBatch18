namespace ComicReader.DTOs;
public class UpdatePageDto
{
    public int Id { get; set; }
    public int PageNumber { get; set; }
    public string PageUrl { get; set; } = string.Empty;
}