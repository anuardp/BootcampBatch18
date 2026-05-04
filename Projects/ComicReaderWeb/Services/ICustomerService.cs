using ComicReader.Models;
using ComicReader.DTOs;

namespace ComicReader.Services;

public interface ICustomerService
{
    Task<ApiResponseDto<UserDto>> RegisterAsync(RegisterDto registerDto);
    Task<ApiResponseDto<UserDto>> LoginAsync(LoginDto loginDto);
    Task<ApiResponseDto<UserDto>> GetByIdAsync(int id);
    Task<ApiResponseDto<UserDto>> GetByEmailAsync(string email);
    Task<ApiResponseDto<List<UserDto>>> GetAllCustomersAsync(string currentUserRole);
    Task<ApiResponseDto<bool>> DeleteCustomerAsync(int id, string currentUserRole);
    Task<ApiResponseDto<bool>> UpdateSubscriptionStatusAsync(int customerId, bool isSubscribed, DateTime? endDate);
}

