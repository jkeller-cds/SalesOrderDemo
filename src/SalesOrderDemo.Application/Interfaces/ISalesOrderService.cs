using SalesOrderDemo.Application.DTOs;

namespace SalesOrderDemo.Application.Interfaces;

public interface ISalesOrderService
{
    Task<IEnumerable<SalesOrderDto>> GetAllOrdersAsync();
    Task<SalesOrderDto?> GetOrderByIdAsync(int id);
    Task<SalesOrderDto> CreateOrderAsync(SalesOrderDto orderDto);
    Task UpdateOrderAsync(SalesOrderDto orderDto);
    Task DeleteOrderAsync(int id);
}
