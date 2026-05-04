using ComicReader.Models;
using ComicReader.DTOs;

namespace ComicReader.Services;

public interface ISubscribeHistoryService
{
    Task<ApiResponseDto<SubscribeHistoryDto>> SubscribeAsync(SubscribeToPremiumDto dto);
    Task<ApiResponseDto<List<SubscribeHistoryDto>>> GetHistoriesByCustomerIdAsync(int customerId);
    Task<ApiResponseDto<List<SubscribeHistoryDto>>> GetAllHistoriesAsync(string currentUserRole);
    Task<ApiResponseDto<bool>> UpdateExpiredSubscriptionsAsync();
}