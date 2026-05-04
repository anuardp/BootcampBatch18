using AutoMapper;
using ComicReader.Models;
using ComicReader.DTOs;
using ComicReader.Repositories;

namespace ComicReader.Services;

public class ChapterService : IChapterService
{
    private readonly IChapterRepository _chapterRepo;
    private readonly IComicRepository _comicRepo;
    private readonly IMapper _mapper;

    public ChapterService(IChapterRepository chapterRepo, IComicRepository comicRepo, IMapper mapper)
    {
        _chapterRepo = chapterRepo;
        _comicRepo = comicRepo;
        _mapper = mapper;
    }

    public async Task<ApiResponseDto<Chapter>> AddChapterAsync(AddNewChapterDto dto, string currentUserRole)
    {
        if (currentUserRole != "Admin")
            return ApiResponseDto<Chapter>.ErrorResult("Access denied. Admin only.");

        var exists = await _chapterRepo.ExistsByComicAndNumberAsync(dto.ComicId, dto.ChapterNumber);
        if (exists)
            return ApiResponseDto<Chapter>.ErrorResult("Chapter number already exists for this comic.");

        var comicExists = await _comicRepo.ExistsAsync(dto.ComicId);
        if (!comicExists)
            return ApiResponseDto<Chapter>.ErrorResult("Comic not found.");

        var chapter = _mapper.Map<Chapter>(dto);
        var created = await _chapterRepo.CreateAsync(chapter);
        await _comicRepo.UpdateTotalChapterAsync(dto.ComicId);
        return ApiResponseDto<Chapter>.SuccessResult(created, "Chapter added.");
    }

    public async Task<ApiResponseDto<bool>> RemoveChapterAsync(RemoveChapterDto dto, string currentUserRole)
    {
        if (currentUserRole != "Admin")
            return ApiResponseDto<bool>.ErrorResult("Access denied. Admin only.");

        var chapter = await _chapterRepo.GetByIdAsync(dto.ChapterId);
        if (chapter == null)
            return ApiResponseDto<bool>.ErrorResult("Chapter not found.");

        var comicId = chapter.ComicId;
        var deleted = await _chapterRepo.DeleteAsync(dto.ChapterId);
        if (deleted)
            await _comicRepo.UpdateTotalChapterAsync(comicId);
        return ApiResponseDto<bool>.SuccessResult(deleted, deleted ? "Chapter removed." : "Failed to remove chapter.");
    }

    public async Task<ApiResponseDto<List<Chapter>>> GetChaptersByComicIdAsync(int comicId)
    {
        var chapters = await _chapterRepo.GetByComicIdAsync(comicId);
        return ApiResponseDto<List<Chapter>>.SuccessResult(chapters);
    }

    public async Task<ApiResponseDto<Chapter>> GetByIdAsync(int id)
    {
        var chapter = await _chapterRepo.GetByIdAsync(id);
        if (chapter == null)
            return ApiResponseDto<Chapter>.ErrorResult("Chapter not found.");
        return ApiResponseDto<Chapter>.SuccessResult(chapter);
    }
}