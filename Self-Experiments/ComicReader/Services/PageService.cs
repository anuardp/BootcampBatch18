using ComicReader.Data;
using ComicReader.Models;
using Microsoft.EntityFrameworkCore;

namespace ComicReader.Services
{
    public class PageService
    {
        private readonly AppDbContext _context;
        public PageService(AppDbContext context) => _context = context;

        public async Task<Page> CreatePageAsync(Page page)
        {
            if (await _context.Pages.AnyAsync(p => p.ChapterId == page.ChapterId && p.PageNumber == page.PageNumber))
                throw new InvalidOperationException("Page number already exists in this chapter");
            _context.Pages.Add(page);
            await _context.SaveChangesAsync();
            await UpdateChapterTotalPage(page.ChapterId);
            return page;
        }

        public async Task<Page?> GetPageByIdAsync(int id) =>
            await _context.Pages.Include(p => p.Chapter).FirstOrDefaultAsync(p => p.Id == id);

        public async Task<List<Page>> GetPagesByChapterIdAsync(int chapterId) =>
            await _context.Pages.Where(p => p.ChapterId == chapterId).OrderBy(p => p.PageNumber).ToListAsync();

        public async Task<bool> UpdatePageAsync(int id, Page updated)
        {
            var page = await _context.Pages.FindAsync(id);
            if (page == null) return false;
            page.PageNumber = updated.PageNumber;
            page.PageUrl = updated.PageUrl;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeletePageAsync(int id)
        {
            var page = await _context.Pages.FindAsync(id);
            if (page == null) return false;
            int chapterId = page.ChapterId;
            _context.Pages.Remove(page);
            await _context.SaveChangesAsync();
            await UpdateChapterTotalPage(chapterId);
            return true;
        }
        private async Task UpdateChapterTotalPage(int chapterId)
        {
            var chapter = await _context.Chapters.FindAsync(chapterId);
            if (chapter != null)
            {
                chapter.TotalPage = await _context.Pages.CountAsync(p => p.ChapterId == chapterId);
                await _context.SaveChangesAsync();
            }
        }
    }
}