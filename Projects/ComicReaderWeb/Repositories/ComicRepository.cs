using Microsoft.EntityFrameworkCore;
using ComicReader.Data;
using ComicReader.Models;
using ComicReader.DTOs;

namespace ComicReader.Repositories;

public class ComicRepository : IComicRepository
{
    private readonly AppDbContext _context;

    public ComicRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Comic?> GetByIdAsync(int id)
    {
        return await _context.Comics.FindAsync(id);
    }

    public async Task<Comic?> GetByIdWithChaptersAsync(int id)
    {
        return await _context.Comics
            .Include(c => c.Chapters)
            .ThenInclude(ch => ch.Pages)
            .FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task<List<Comic>> GetAllAsync()
    {
        return await _context.Comics
            .OrderBy(c => c.Title)
            .ToListAsync();
    }

    public async Task<List<Comic>> GetFreeComicsAsync()
    {
        return await _context.Comics
            .Where(c => !c.IsPremium)
            .OrderBy(c => c.Title)
            .ToListAsync();
    }

    public async Task<List<Comic>> GetPremiumComicsAsync()
    {
        return await _context.Comics
            .Where(c => c.IsPremium)
            .OrderBy(c => c.Title)
            .ToListAsync();
    }

    public async Task<Comic> CreateAsync(Comic comic)
    {
        _context.Comics.Add(comic);
        await _context.SaveChangesAsync();
        return comic;
    }

    public async Task<Comic> UpdateAsync(Comic comic)
    {
        _context.Comics.Update(comic);
        await _context.SaveChangesAsync();
        return comic;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var comic = await GetByIdAsync(id);
        if (comic == null) return false;
        _context.Comics.Remove(comic);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> ExistsAsync(int id)
    {
        return await _context.Comics.AnyAsync(c => c.Id == id);
    }

    public async Task<int> GetTotalChapterCountAsync(int comicId)
    {
        return await _context.Chapters.CountAsync(ch => ch.ComicId == comicId);
    }

    public async Task UpdateTotalChapterAsync(int comicId)
    {
        var comic = await GetByIdAsync(comicId);
        if (comic != null)
        {
            comic.TotalChapter = await GetTotalChapterCountAsync(comicId);
            await _context.SaveChangesAsync();
        }
    }
}