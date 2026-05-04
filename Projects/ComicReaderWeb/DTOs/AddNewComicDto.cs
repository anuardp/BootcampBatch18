namespace ComicReader.DTOs;

public class AddNewComicDto
{
    public string Title { get; set; } = string.Empty;
    public string? Publisher { get; set; }
    public string? Author { get; set; }
    public string? Genre { get; set; }
    public int? YearReleased { get; set; }
    public bool IsOnGoing { get; set; } = true;
    public bool IsPremium { get; set; }
    
}