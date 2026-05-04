using ComicReader.Data;
using ComicReader.Models;
using Microsoft.EntityFrameworkCore;

namespace ComicReader.Services
{
    public class ComicService
    {
        private readonly AppDbContext _context;
        public ComicService(AppDbContext context) => _context = context;

        public async Task<Comic> CreateComicAsync(Comic comic)
        {
            _context.Comics.Add(comic);
            await _context.SaveChangesAsync();
            return comic;
        }

        public async Task<List<Comic>> GetAllComicsAsync()
        {
            return await _context.Comics.Include(c => c.Chapters).OrderBy(c => c.Title).ToListAsync();
        }

        public async Task<Comic?> GetComicByIdAsync(int id)
        {
            return await _context.Comics.Include(c => c.Chapters).ThenInclude(ch => ch.Pages).FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<Comic?> UpdateComicAsync(int id, Comic updated)
        {
            var comic = await _context.Comics.FindAsync(id);
            if (comic == null) return null;
            comic.Title = updated.Title;
            comic.Publisher = updated.Publisher;
            comic.Author = updated.Author;
            comic.Genre = updated.Genre;
            comic.YearReleased = updated.YearReleased;
            comic.IsOnGoing = updated.IsOnGoing;
            comic.IsPremium = updated.IsPremium;
            comic.TotalChapter = updated.TotalChapter;
            await _context.SaveChangesAsync();
            return comic;
        }

        public async Task<bool> DeleteComicAsync(int id)
        {
            var comic = await _context.Comics.Include(c => c.Chapters).FirstOrDefaultAsync(c => c.Id == id);
            if (comic == null) return false;
            _context.Comics.Remove(comic);
            await _context.SaveChangesAsync();
            return true;
        }
        public async Task<int> RecalculateTotalChapterAsync(int comicId)
        {
            var comic = await _context.Comics.FindAsync(comicId);
            if (comic == null) throw new ArgumentException("Comic not found");
            comic.TotalChapter = await _context.Chapters.CountAsync(ch => ch.ComicId == comicId);
            await _context.SaveChangesAsync();
            return comic.TotalChapter;
        }
    }
}