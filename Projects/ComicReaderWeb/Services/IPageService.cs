using ComicReader.Models;
using ComicReader.DTOs;

namespace ComicReader.Services;

public interface IPageService
{
    Task<ApiResponseDto<Page>> AddPageAsync(AddPageDto dto, string currentUserRole);
    Task<ApiResponseDto<Page>> UpdatePageAsync(UpdatePageDto dto, string currentUserRole);
    Task<ApiResponseDto<bool>> RemovePageAsync(RemovePageDto dto, string currentUserRole);
    Task<ApiResponseDto<List<Page>>> GetPagesByChapterIdAsync(int chapterId);
    Task<ApiResponseDto<Page>> GetByIdAsync(int id);
}