using ComicReader.Models;
using ComicReader.DTOs;

namespace ComicReader.Repositories;

public interface IChapterRepository
{
    Task<Chapter?> GetByIdAsync(int id);
    Task<Chapter?> GetByIdWithPagesAsync(int id);
    Task<List<Chapter>> GetByComicIdAsync(int comicId);
    Task<Chapter?> GetByComicAndNumberAsync(int comicId, int chapterNumber);
    Task<Chapter> CreateAsync(Chapter chapter);
    Task<Chapter> UpdateAsync(Chapter chapter);
    Task<bool> DeleteAsync(int id);
    Task<bool> ExistsAsync(int id);
    Task<bool> ExistsByComicAndNumberAsync(int comicId, int chapterNumber);
    Task<int> GetTotalPageCountAsync(int chapterId);
    Task UpdateTotalPageAsync(int chapterId);
}