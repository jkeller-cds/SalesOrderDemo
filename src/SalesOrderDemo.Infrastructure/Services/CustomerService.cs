using Microsoft.EntityFrameworkCore;
using SalesOrderDemo.Application.DTOs;
using SalesOrderDemo.Application.Interfaces;
using SalesOrderDemo.Domain.Entities;
using SalesOrderDemo.Domain.Interfaces;
using SalesOrderDemo.Infrastructure.Data;

namespace SalesOrderDemo.Infrastructure.Services;

public class CustomerService : ICustomerService
{
    private readonly IRepository<Customer> _customerRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ApplicationDbContext _context;

    public CustomerService(
        IRepository<Customer> customerRepository, 
        IUnitOfWork unitOfWork,
        ApplicationDbContext context)
    {
        _customerRepository = customerRepository;
        _unitOfWork = unitOfWork;
        _context = context;
    }

    public async Task<IEnumerable<CustomerDto>> GetAllCustomersAsync()
    {
        var customers = await _customerRepository.GetAllAsync();
        return customers.Select(c => MapToDto(c));
    }

    public async Task<CustomerDto?> GetCustomerByIdAsync(int id)
    {
        var customer = await _customerRepository.GetByIdAsync(id);
        return customer != null ? MapToDto(customer) : null;
    }

    public async Task<CustomerDto> CreateCustomerAsync(CustomerDto customerDto)
    {
        var customer = MapToEntity(customerDto);
        await _customerRepository.AddAsync(customer);
        await _unitOfWork.SaveChangesAsync();
        return MapToDto(customer);
    }

    public async Task UpdateCustomerAsync(CustomerDto customerDto)
    {
        var customer = MapToEntity(customerDto);
        await _customerRepository.UpdateAsync(customer);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task DeleteCustomerAsync(int id)
    {
        var customer = await _customerRepository.GetByIdAsync(id);
        if (customer != null)
        {
            await _customerRepository.DeleteAsync(customer);
            await _unitOfWork.SaveChangesAsync();
        }
    }

    private static CustomerDto MapToDto(Customer customer)
    {
        return new CustomerDto
        {
            Id = customer.Id,
            Name = customer.Name,
            Email = customer.Email,
            Phone = customer.Phone,
            Address = customer.Address,
            City = customer.City,
            Country = customer.Country
        };
    }

    private static Customer MapToEntity(CustomerDto dto)
    {
        return new Customer
        {
            Id = dto.Id,
            Name = dto.Name,
            Email = dto.Email,
            Phone = dto.Phone,
            Address = dto.Address,
            City = dto.City,
            Country = dto.Country
        };
    }
}
