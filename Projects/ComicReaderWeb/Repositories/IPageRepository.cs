using ComicReader.Models;
using ComicReader.DTOs;

namespace ComicReader.Repositories;

public interface IPageRepository
{
    Task<Page?> GetByIdAsync(int id);
    Task<List<Page>> GetByChapterIdAsync(int chapterId);
    Task<Page?> GetByChapterAndNumberAsync(int chapterId, int pageNumber);
    Task<Page> CreateAsync(Page page);
    Task<Page> UpdateAsync(Page page);
    Task<bool> DeleteAsync(int id);
    Task<bool> ExistsAsync(int id);
    Task<int> GetMaxPageNumberAsync(int chapterId);
}