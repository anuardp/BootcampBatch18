using ComicReader.Data;
using ComicReader.Models;
using Microsoft.EntityFrameworkCore;

namespace ComicReader.Services
{
    public class ChapterService
    {
        private readonly AppDbContext _context;
        public ChapterService(AppDbContext context) => _context = context;

        public async Task<Chapter> CreateChapterAsync(Chapter chapter)
        {
            if (await _context.Chapters.AnyAsync(ch => ch.ComicId == chapter.ComicId && ch.ChapterNumber == chapter.ChapterNumber))
                throw new InvalidOperationException("Chapter number already exists for this comic");
            _context.Chapters.Add(chapter);
            await _context.SaveChangesAsync();
            await UpdateComicTotalChapter(chapter.ComicId);
            return chapter;
        }
        public async Task<Chapter?> GetChapterByIdAsync(int id) =>
            await _context.Chapters.Include(ch => ch.Pages).Include(ch => ch.Comic).FirstOrDefaultAsync(ch => ch.Id == id);

        public async Task<List<Chapter>> GetChaptersByComicIdAsync(int comicId) =>
            await _context.Chapters.Where(ch => ch.ComicId == comicId).OrderBy(ch => ch.ChapterNumber).ToListAsync();

        public async Task<bool> UpdateChapterAsync(int id, Chapter updated)
        {
            var chapter = await _context.Chapters.FindAsync(id);
            if (chapter == null) return false;
            chapter.ChapterNumber = updated.ChapterNumber;
            chapter.DateUploaded = updated.DateUploaded;
            chapter.TotalPage = updated.TotalPage;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteChapterAsync(int id)
        {
            var chapter = await _context.Chapters.FindAsync(id);
            if (chapter == null) return false;
            int comicId = chapter.ComicId;
            _context.Chapters.Remove(chapter);
            await _context.SaveChangesAsync();
            await UpdateComicTotalChapter(comicId);
            return true;
        }

        private async Task UpdateComicTotalChapter(int comicId)
        {
            var comic = await _context.Comics.FindAsync(comicId);
            if (comic != null)
            {
                comic.TotalChapter = await _context.Chapters.CountAsync(ch => ch.ComicId == comicId);
                await _context.SaveChangesAsync();
            }
        }
        public async Task<int> RecalculateTotalPageAsync(int chapterId)
        {
            var chapter = await _context.Chapters.FindAsync(chapterId);
            if (chapter == null) throw new ArgumentException("Chapter not found");
            chapter.TotalPage = await _context.Pages.CountAsync(p => p.ChapterId == chapterId);
            await _context.SaveChangesAsync();
            return chapter.TotalPage;
        }
    }
}