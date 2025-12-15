using SalesOrderDemo.Application.DTOs;

namespace SalesOrderDemo.Application.Interfaces;

public interface ICustomerService
{
    Task<IEnumerable<CustomerDto>> GetAllCustomersAsync();
    Task<CustomerDto?> GetCustomerByIdAsync(int id);
    Task<CustomerDto> CreateCustomerAsync(CustomerDto customerDto);
    Task UpdateCustomerAsync(CustomerDto customerDto);
    Task DeleteCustomerAsync(int id);
}
