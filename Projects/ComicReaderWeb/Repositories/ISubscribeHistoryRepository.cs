using ComicReader.Models;
using ComicReader.DTOs;

namespace ComicReader.Repositories;

public interface ISubscribeHistoryRepository
{
    Task<SubscribeHistory?> GetByIdAsync(int id);
    Task<List<SubscribeHistory>> GetByCustomerIdAsync(int customerId);
    Task<List<SubscribeHistory>> GetAllAsync();
    Task<SubscribeHistory> CreateAsync(SubscribeHistory history);
    Task<bool> DeleteAsync(int id);
    Task<List<SubscribeHistory>> GetActiveSubscriptionsAsync();
}