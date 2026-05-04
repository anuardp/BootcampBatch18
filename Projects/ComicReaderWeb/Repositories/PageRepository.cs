using Microsoft.EntityFrameworkCore;
using ComicReader.Data;
using ComicReader.Models;
using ComicReader.DTOs;

namespace ComicReader.Repositories;

public class PageRepository : IPageRepository
{
    private readonly AppDbContext _context;

    public PageRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Page?> GetByIdAsync(int id)
    {
        return await _context.Pages.FindAsync(id);
    }

    public async Task<List<Page>> GetByChapterIdAsync(int chapterId)
    {
        return await _context.Pages
            .Where(p => p.ChapterId == chapterId)
            .OrderBy(p => p.PageNumber)
            .ToListAsync();
    }

    public async Task<Page?> GetByChapterAndNumberAsync(int chapterId, int pageNumber)
    {
        return await _context.Pages
            .FirstOrDefaultAsync(p => p.ChapterId == chapterId && p.PageNumber == pageNumber);
    }

    public async Task<Page> CreateAsync(Page page)
    {
        _context.Pages.Add(page);
        await _context.SaveChangesAsync();
        return page;
    }

    public async Task<Page> UpdateAsync(Page page)
    {
        _context.Pages.Update(page);
        await _context.SaveChangesAsync();
        return page;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var page = await GetByIdAsync(id);
        if (page == null) return false;
        _context.Pages.Remove(page);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> ExistsAsync(int id)
    {
        return await _context.Pages.AnyAsync(p => p.Id == id);
    }

    public async Task<int> GetMaxPageNumberAsync(int chapterId)
    {
        if (!await _context.Pages.AnyAsync(p => p.ChapterId == chapterId))
            return 0;
        return await _context.Pages
            .Where(p => p.ChapterId == chapterId)
            .MaxAsync(p => p.PageNumber);
    }
}