using AutoMapper;
using ComicReader.Models;
using ComicReader.DTOs;
using ComicReader.Repositories;

namespace ComicReader.Services;

public class SubscribeHistoryService : ISubscribeHistoryService
{
    private readonly ISubscribeHistoryRepository _historyRepo;
    private readonly ICustomerRepository _customerRepo;
    private readonly IMapper _mapper;

    public SubscribeHistoryService(ISubscribeHistoryRepository historyRepo, ICustomerRepository customerRepo, IMapper mapper)
    {
        _historyRepo = historyRepo;
        _customerRepo = customerRepo;
        _mapper = mapper;
    }

    public async Task<ApiResponseDto<SubscribeHistoryDto>> SubscribeAsync(SubscribeToPremiumDto dto)
    {
        var customer = await _customerRepo.GetByIdAsync(dto.CustomerId);
        if (customer == null)
            return ApiResponseDto<SubscribeHistoryDto>.ErrorResult("Customer not found.");

        var start = DateTime.UtcNow;
        var end = start.AddDays(dto.DurationDays);
        var history = new SubscribeHistory
        {
            CustomerId = dto.CustomerId,
            StartDate = start,
            EndDate = end,
            TransactionDate = DateTime.UtcNow,
            PaymentMethod = dto.PaymentMethod
        };
        var created = await _historyRepo.CreateAsync(history);

        // Update customer subscription status
        if (customer.SubscribeEndDate.HasValue && customer.SubscribeEndDate.Value > start)
            customer.SubscribeEndDate = customer.SubscribeEndDate.Value.AddDays(dto.DurationDays);
        else
            customer.SubscribeEndDate = end;
        customer.IsSubscribe = true;
        await _customerRepo.UpdateAsync(customer);

        var dtoResult = _mapper.Map<SubscribeHistoryDto>(created);
        return ApiResponseDto<SubscribeHistoryDto>.SuccessResult(dtoResult, "Subscription successful.");
    }

    public async Task<ApiResponseDto<List<SubscribeHistoryDto>>> GetHistoriesByCustomerIdAsync(int customerId)
    {
        var histories = await _historyRepo.GetByCustomerIdAsync(customerId);
        var dtos = _mapper.Map<List<SubscribeHistoryDto>>(histories);
        return ApiResponseDto<List<SubscribeHistoryDto>>.SuccessResult(dtos);
    }

    public async Task<ApiResponseDto<List<SubscribeHistoryDto>>> GetAllHistoriesAsync(string currentUserRole)
    {
        if (currentUserRole != "Admin")
            return ApiResponseDto<List<SubscribeHistoryDto>>.ErrorResult("Access denied. Admin only.");
        var histories = await _historyRepo.GetAllAsync();
        var dtos = _mapper.Map<List<SubscribeHistoryDto>>(histories);
        return ApiResponseDto<List<SubscribeHistoryDto>>.SuccessResult(dtos);
    }

    public async Task<ApiResponseDto<bool>> UpdateExpiredSubscriptionsAsync()
    {
        var now = DateTime.UtcNow;
        var customers = await _customerRepo.GetAllAsync();
        var updated = false;
        foreach (var c in customers)
        {
            if (c.IsSubscribe && c.SubscribeEndDate < now)
            {
                c.IsSubscribe = false;
                await _customerRepo.UpdateAsync(c);
                updated = true;
            }
        }
        return ApiResponseDto<bool>.SuccessResult(updated, "Expired subscriptions updated.");
    }
}