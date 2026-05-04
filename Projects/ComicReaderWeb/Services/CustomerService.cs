using AutoMapper;
using ComicReader.Models;
using ComicReader.DTOs;
using ComicReader.Repositories;
using Microsoft.AspNetCore.Identity;

namespace ComicReader.Services;

public class CustomerService : ICustomerService
{
    private readonly ICustomerRepository _customerRepo;
    private readonly IMapper _mapper;
    private readonly IPasswordHasher<Customer> _passwordHasher;

    public CustomerService(ICustomerRepository customerRepo, IMapper mapper, IPasswordHasher<Customer> passwordHasher)
    {
        _customerRepo = customerRepo;
        _mapper = mapper;
        _passwordHasher = passwordHasher;
    }

    public async Task<ApiResponseDto<UserDto>> RegisterAsync(RegisterDto registerDto)
    {
        try
        {
            if (await _customerRepo.ExistsByEmailAsync(registerDto.Email))
                return ApiResponseDto<UserDto>.ErrorResult("Email already registered.");

            var customer = _mapper.Map<Customer>(registerDto);
            customer.Role = "User";
            customer.IsSubscribe = false;
            customer.SubscribeEndDate = null;

            // Hash password (simpan di properti PasswordHash; asumsikan ada)
            // Untuk keperluan demo, kita tambahkan properti PasswordHash di Customer.cs
            customer.PasswordHash = _passwordHasher.HashPassword(customer, registerDto.Password);

            var created = await _customerRepo.CreateAsync(customer);
            var userDto = _mapper.Map<UserDto>(created);
            return ApiResponseDto<UserDto>.SuccessResult(userDto, "Registration successful.");
        }
        catch (Exception ex)
        {
            return ApiResponseDto<UserDto>.ErrorResult($"Registration failed: {ex.Message}");
        }
    }

    public async Task<ApiResponseDto<UserDto>> LoginAsync(LoginDto loginDto)
    {
        try
        {
            var customer = await _customerRepo.GetByEmailAsync(loginDto.Email);
            if (customer == null)
                return ApiResponseDto<UserDto>.ErrorResult("Invalid email or password.");

            var result = _passwordHasher.VerifyHashedPassword(customer, customer.PasswordHash, loginDto.Password);
            if (result == PasswordVerificationResult.Failed)
                return ApiResponseDto<UserDto>.ErrorResult("Invalid email or password.");

            var userDto = _mapper.Map<UserDto>(customer);
            return ApiResponseDto<UserDto>.SuccessResult(userDto, "Login successful.");
        }
        catch (Exception ex)
        {
            return ApiResponseDto<UserDto>.ErrorResult($"Login failed: {ex.Message}");
        }
    }

    public async Task<ApiResponseDto<UserDto>> GetByIdAsync(int id)
    {
        var customer = await _customerRepo.GetByIdAsync(id);
        if (customer == null)
            return ApiResponseDto<UserDto>.ErrorResult("Customer not found.");
        return ApiResponseDto<UserDto>.SuccessResult(_mapper.Map<UserDto>(customer));
    }

    public async Task<ApiResponseDto<UserDto>> GetByEmailAsync(string email)
    {
        var customer = await _customerRepo.GetByEmailAsync(email);
        if (customer == null)
            return ApiResponseDto<UserDto>.ErrorResult("Customer not found.");
        return ApiResponseDto<UserDto>.SuccessResult(_mapper.Map<UserDto>(customer));
    }

    public async Task<ApiResponseDto<List<UserDto>>> GetAllCustomersAsync(string currentUserRole)
    {
        if (currentUserRole != "Admin")
            return ApiResponseDto<List<UserDto>>.ErrorResult("Access denied. Admin only.");

        var customers = await _customerRepo.GetAllAsync();
        var dtos = _mapper.Map<List<UserDto>>(customers);
        return ApiResponseDto<List<UserDto>>.SuccessResult(dtos);
    }

    public async Task<ApiResponseDto<bool>> DeleteCustomerAsync(int id, string currentUserRole)
    {
        if (currentUserRole != "Admin")
            return ApiResponseDto<bool>.ErrorResult("Access denied. Admin only.");

        var deleted = await _customerRepo.DeleteAsync(id);
        if (!deleted)
            return ApiResponseDto<bool>.ErrorResult("Customer not found.");
        return ApiResponseDto<bool>.SuccessResult(true, "Customer deleted.");
    }

    public async Task<ApiResponseDto<bool>> UpdateSubscriptionStatusAsync(int customerId, bool isSubscribed, DateTime? endDate)
    {
        var customer = await _customerRepo.GetByIdAsync(customerId);
        if (customer == null)
            return ApiResponseDto<bool>.ErrorResult("Customer not found.");

        customer.IsSubscribe = isSubscribed;
        customer.SubscribeEndDate = endDate;
        await _customerRepo.UpdateAsync(customer);
        return ApiResponseDto<bool>.SuccessResult(true, "Subscription status updated.");
    }
}