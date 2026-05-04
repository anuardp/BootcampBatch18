using ComicReader.Models;
using ComicReader.DTOs;

namespace ComicReader.Services;

public interface IComicService
{
    Task<ApiResponseDto<Comic>> AddComicAsync(AddNewComicDto dto, string currentUserRole);
    Task<ApiResponseDto<bool>> RemoveComicAsync(RemoveComicDto dto, string currentUserRole);
    Task<ApiResponseDto<List<Comic>>> GetAllComicsAsync();
    Task<ApiResponseDto<ReadComicDto>> GetComicForReadingAsync(int comicId, int? chapterId, int customerId, bool isSubscribed);
    Task<ApiResponseDto<Comic>> GetByIdAsync(int id);
}