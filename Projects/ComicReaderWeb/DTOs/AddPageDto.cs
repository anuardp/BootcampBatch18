namespace ComicReader.DTOs;
public class AddPageDto
{
    public int ChapterId { get; set; }
    public int PageNumber { get; set; }
    public string ImageUrl { get; set; } = string.Empty;
}