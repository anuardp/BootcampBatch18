using ComicReader.Models;
using ComicReader.DTOs;

namespace ComicReader.Services;

public interface IChapterService
{
    Task<ApiResponseDto<Chapter>> AddChapterAsync(AddNewChapterDto dto, string currentUserRole);
    Task<ApiResponseDto<bool>> RemoveChapterAsync(RemoveChapterDto dto, string currentUserRole);
    Task<ApiResponseDto<List<Chapter>>> GetChaptersByComicIdAsync(int comicId);
    Task<ApiResponseDto<Chapter>> GetByIdAsync(int id);
}