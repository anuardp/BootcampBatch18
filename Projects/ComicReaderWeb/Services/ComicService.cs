using AutoMapper;
using ComicReader.Models;
using ComicReader.DTOs;
using ComicReader.Repositories;

namespace ComicReader.Services;

public class ComicService : IComicService
{
    private readonly IComicRepository _comicRepo;
    private readonly IChapterRepository _chapterRepo;
    private readonly IPageRepository _pageRepo;
    private readonly IMapper _mapper;

    public ComicService(IComicRepository comicRepo, IChapterRepository chapterRepo, IPageRepository pageRepo, IMapper mapper)
    {
        _comicRepo = comicRepo;
        _chapterRepo = chapterRepo;
        _pageRepo = pageRepo;
        _mapper = mapper;
    }

    public async Task<ApiResponseDto<Comic>> AddComicAsync(AddNewComicDto dto, string currentUserRole)
    {
        if (currentUserRole != "Admin")
            return ApiResponseDto<Comic>.ErrorResult("Access denied. Admin only.");

        var comic = _mapper.Map<Comic>(dto);
        var created = await _comicRepo.CreateAsync(comic);
        return ApiResponseDto<Comic>.SuccessResult(created, "Comic added.");
    }

    public async Task<ApiResponseDto<bool>> RemoveComicAsync(RemoveComicDto dto, string currentUserRole)
    {
        if (currentUserRole != "Admin")
            return ApiResponseDto<bool>.ErrorResult("Access denied. Admin only.");

        var deleted = await _comicRepo.DeleteAsync(dto.ComicId);
        if (!deleted)
            return ApiResponseDto<bool>.ErrorResult("Comic not found.");
        return ApiResponseDto<bool>.SuccessResult(true, "Comic removed.");
    }

    public async Task<ApiResponseDto<List<Comic>>> GetAllComicsAsync()
    {
        var comics = await _comicRepo.GetAllAsync();
        return ApiResponseDto<List<Comic>>.SuccessResult(comics);
    }

    public async Task<ApiResponseDto<ReadComicDto>> GetComicForReadingAsync(int comicId, int? chapterId, int customerId, bool isSubscribed)
    {
        var comic = await _comicRepo.GetByIdWithChaptersAsync(comicId);
        if (comic == null)
            return ApiResponseDto<ReadComicDto>.ErrorResult("Comic not found.");

        // Authorization
        if (comic.IsPremium && !isSubscribed)
            return ApiResponseDto<ReadComicDto>.ErrorResult("This comic requires a subscription.");

        var readDto = _mapper.Map<ReadComicDto>(comic);
        
        // Determine chapter to display
        Chapter? targetChapter = null;
        if (chapterId.HasValue && chapterId.Value > 0)
            targetChapter = comic.Chapters.FirstOrDefault(ch => ch.Id == chapterId.Value);
        else
            targetChapter = comic.Chapters.OrderBy(ch => ch.ChapterNumber).FirstOrDefault();

        if (targetChapter == null)
        {
            readDto.SelectedChapterId = 0;
            readDto.Pages = new List<PageInfo>();
            return ApiResponseDto<ReadComicDto>.SuccessResult(readDto, "No chapters available.");
        }

        readDto.SelectedChapterId = targetChapter.Id;
        var pages = await _pageRepo.GetByChapterIdAsync(targetChapter.Id);
        readDto.Pages = _mapper.Map<List<PageInfo>>(pages);

        return ApiResponseDto<ReadComicDto>.SuccessResult(readDto);
    }

    public async Task<ApiResponseDto<Comic>> GetByIdAsync(int id)
    {
        var comic = await _comicRepo.GetByIdAsync(id);
        if (comic == null)
            return ApiResponseDto<Comic>.ErrorResult("Comic not found.");
        return ApiResponseDto<Comic>.SuccessResult(comic);
    }
}