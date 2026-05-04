namespace ComicReader.DTOs;
public class UpdatePageDto
{
    public int Id { get; set; }
    public int PageNumber { get; set; }
    public string ImageUrl { get; set; } = string.Empty;
}