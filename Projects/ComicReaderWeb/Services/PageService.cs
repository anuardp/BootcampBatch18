using AutoMapper;
using ComicReader.Models;
using ComicReader.DTOs;
using ComicReader.Repositories;

namespace ComicReader.Services;

public class PageService : IPageService
{
    private readonly IPageRepository _pageRepo;
    private readonly IChapterRepository _chapterRepo;
    private readonly IMapper _mapper;

    public PageService(IPageRepository pageRepo, IChapterRepository chapterRepo, IMapper mapper)
    {
        _pageRepo = pageRepo;
        _chapterRepo = chapterRepo;
        _mapper = mapper;
    }

    public async Task<ApiResponseDto<Page>> AddPageAsync(AddPageDto dto, string currentUserRole)
    {
        if (currentUserRole != "Admin")
            return ApiResponseDto<Page>.ErrorResult("Access denied. Admin only.");

        var chapterExists = await _chapterRepo.ExistsAsync(dto.ChapterId);
        if (!chapterExists)
            return ApiResponseDto<Page>.ErrorResult("Chapter not found.");

        var pageExists = await _pageRepo.GetByChapterAndNumberAsync(dto.ChapterId, dto.PageNumber);
        if (pageExists != null)
            return ApiResponseDto<Page>.ErrorResult("Page number already exists in this chapter.");

        var page = _mapper.Map<Page>(dto);
        var created = await _pageRepo.CreateAsync(page);
        await _chapterRepo.UpdateTotalPageAsync(dto.ChapterId);
        return ApiResponseDto<Page>.SuccessResult(created, "Page added.");
    }

    public async Task<ApiResponseDto<Page>> UpdatePageAsync(UpdatePageDto dto, string currentUserRole)
    {
        if (currentUserRole != "Admin")
            return ApiResponseDto<Page>.ErrorResult("Access denied. Admin only.");

        var existing = await _pageRepo.GetByIdAsync(dto.Id);
        if (existing == null)
            return ApiResponseDto<Page>.ErrorResult("Page not found.");

        _mapper.Map(dto, existing); // hanya update PageNumber dan ImageUrl (ChapterId tidak boleh berubah)
        var updated = await _pageRepo.UpdateAsync(existing);
        return ApiResponseDto<Page>.SuccessResult(updated, "Page updated.");
    }

    public async Task<ApiResponseDto<bool>> RemovePageAsync(RemovePageDto dto, string currentUserRole)
    {
        if (currentUserRole != "Admin")
            return ApiResponseDto<bool>.ErrorResult("Access denied. Admin only.");

        var page = await _pageRepo.GetByIdAsync(dto.PageId);
        if (page == null)
            return ApiResponseDto<bool>.ErrorResult("Page not found.");

        var chapterId = page.ChapterId;
        var deleted = await _pageRepo.DeleteAsync(dto.PageId);
        if (deleted)
            await _chapterRepo.UpdateTotalPageAsync(chapterId);
        return ApiResponseDto<bool>.SuccessResult(deleted, deleted ? "Page removed." : "Failed to remove page.");
    }

    public async Task<ApiResponseDto<List<Page>>> GetPagesByChapterIdAsync(int chapterId)
    {
        var pages = await _pageRepo.GetByChapterIdAsync(chapterId);
        return ApiResponseDto<List<Page>>.SuccessResult(pages);
    }

    public async Task<ApiResponseDto<Page>> GetByIdAsync(int id)
    {
        var page = await _pageRepo.GetByIdAsync(id);
        if (page == null)
            return ApiResponseDto<Page>.ErrorResult("Page not found.");
        return ApiResponseDto<Page>.SuccessResult(page);
    }
}