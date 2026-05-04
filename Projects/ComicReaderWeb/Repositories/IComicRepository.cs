using ComicReader.Models;
using ComicReader.DTOs;

namespace ComicReader.Repositories;
public interface IComicRepository
{
    Task<Comic?> GetByIdAsync(int id);
    Task<Comic?> GetByIdWithChaptersAsync(int id);
    Task<List<Comic>> GetAllAsync();
    Task<List<Comic>> GetFreeComicsAsync();    // IsPremium == false
    Task<List<Comic>> GetPremiumComicsAsync(); // IsPremium == true
    Task<Comic> CreateAsync(Comic comic);
    Task<Comic> UpdateAsync(Comic comic);
    Task<bool> DeleteAsync(int id);
    Task<bool> ExistsAsync(int id);
    Task<int> GetTotalChapterCountAsync(int comicId);
    Task UpdateTotalChapterAsync(int comicId);
}