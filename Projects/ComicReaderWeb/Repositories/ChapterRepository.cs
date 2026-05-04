using Microsoft.EntityFrameworkCore;
using ComicReader.Data;
using ComicReader.Models;
using ComicReader.DTOs;

namespace ComicReader.Repositories;

public class ChapterRepository : IChapterRepository
{
    private readonly AppDbContext _context;

    public ChapterRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Chapter?> GetByIdAsync(int id)
    {
        return await _context.Chapters.FindAsync(id);
    }

    public async Task<Chapter?> GetByIdWithPagesAsync(int id)
    {
        return await _context.Chapters
            .Include(ch => ch.Pages)
            .FirstOrDefaultAsync(ch => ch.Id == id);
    }

    public async Task<List<Chapter>> GetByComicIdAsync(int comicId)
    {
        return await _context.Chapters
            .Where(ch => ch.ComicId == comicId)
            .OrderBy(ch => ch.ChapterNumber)
            .ToListAsync();
    }

    public async Task<Chapter?> GetByComicAndNumberAsync(int comicId, int chapterNumber)
    {
        return await _context.Chapters
            .FirstOrDefaultAsync(ch => ch.ComicId == comicId && ch.ChapterNumber == chapterNumber);
    }

    public async Task<Chapter> CreateAsync(Chapter chapter)
    {
        _context.Chapters.Add(chapter);
        await _context.SaveChangesAsync();
        return chapter;
    }

    public async Task<Chapter> UpdateAsync(Chapter chapter)
    {
        _context.Chapters.Update(chapter);
        await _context.SaveChangesAsync();
        return chapter;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var chapter = await GetByIdAsync(id);
        if (chapter == null) return false;
        _context.Chapters.Remove(chapter);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> ExistsAsync(int id)
    {
        return await _context.Chapters.AnyAsync(ch => ch.Id == id);
    }

    public async Task<bool> ExistsByComicAndNumberAsync(int comicId, int chapterNumber)
    {
        return await _context.Chapters.AnyAsync(ch => ch.ComicId == comicId && ch.ChapterNumber == chapterNumber);
    }

    public async Task<int> GetTotalPageCountAsync(int chapterId)
    {
        return await _context.Pages.CountAsync(p => p.ChapterId == chapterId);
    }

    public async Task UpdateTotalPageAsync(int chapterId)
    {
        var chapter = await GetByIdAsync(chapterId);
        if (chapter != null)
        {
            chapter.TotalPage = await GetTotalPageCountAsync(chapterId);
            await _context.SaveChangesAsync();
        }
    }
}