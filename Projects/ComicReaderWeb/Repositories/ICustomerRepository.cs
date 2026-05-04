using ComicReader.Models;
using ComicReader.DTOs;

namespace ComicReader.Repositories;
public interface ICustomerRepository
{
    Task<Customer?> GetByIdAsync(int id);
    Task<Customer?> GetByEmailAsync(string email);
    Task<Customer?> GetByIdWithHistoriesAsync(int id);
    Task<List<Customer>> GetAllAsync();
    Task<Customer> CreateAsync(Customer customer);
    Task<Customer> UpdateAsync(Customer customer);
    Task<bool> DeleteAsync(int id);
    Task<bool> ExistsByEmailAsync(string email);
    Task<List<Customer>> GetSubscribedCustomersAsync();
}