using Microsoft.EntityFrameworkCore;
using SalesOrderDemo.Application.DTOs;
using SalesOrderDemo.Application.Interfaces;
using SalesOrderDemo.Domain.Entities;
using SalesOrderDemo.Domain.Interfaces;
using SalesOrderDemo.Infrastructure.Data;

namespace SalesOrderDemo.Infrastructure.Services;

public class SalesOrderService : ISalesOrderService
{
    private readonly IRepository<SalesOrder> _orderRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ApplicationDbContext _context;

    public SalesOrderService(
        IRepository<SalesOrder> orderRepository, 
        IUnitOfWork unitOfWork,
        ApplicationDbContext context)
    {
        _orderRepository = orderRepository;
        _unitOfWork = unitOfWork;
        _context = context;
    }

    public async Task<IEnumerable<SalesOrderDto>> GetAllOrdersAsync()
    {
        var orders = await _context.SalesOrders
            .Include(o => o.Customer)
            .Include(o => o.OrderItems)
            .ThenInclude(i => i.Product)
            .ToListAsync();

        return orders.Select(o => MapToDto(o));
    }

    public async Task<SalesOrderDto?> GetOrderByIdAsync(int id)
    {
        var order = await _context.SalesOrders
            .Include(o => o.Customer)
            .Include(o => o.OrderItems)
            .ThenInclude(i => i.Product)
            .FirstOrDefaultAsync(o => o.Id == id);

        return order != null ? MapToDto(order) : null;
    }

    public async Task<SalesOrderDto> CreateOrderAsync(SalesOrderDto orderDto)
    {
        var order = new SalesOrder
        {
            OrderNumber = orderDto.OrderNumber,
            OrderDate = orderDto.OrderDate,
            CustomerId = orderDto.CustomerId,
            Status = orderDto.Status,
            TotalAmount = orderDto.TotalAmount,
            Notes = orderDto.Notes,
            OrderItems = orderDto.OrderItems.Select(i => new OrderItem
            {
                ProductId = i.ProductId,
                Quantity = i.Quantity,
                UnitPrice = i.UnitPrice,
                TotalPrice = i.TotalPrice
            }).ToList()
        };

        await _orderRepository.AddAsync(order);
        await _unitOfWork.SaveChangesAsync();

        return await GetOrderByIdAsync(order.Id) ?? orderDto;
    }

    public async Task UpdateOrderAsync(SalesOrderDto orderDto)
    {
        var order = await _context.SalesOrders
            .Include(o => o.OrderItems)
            .FirstOrDefaultAsync(o => o.Id == orderDto.Id);

        if (order != null)
        {
            order.OrderNumber = orderDto.OrderNumber;
            order.OrderDate = orderDto.OrderDate;
            order.CustomerId = orderDto.CustomerId;
            order.Status = orderDto.Status;
            order.TotalAmount = orderDto.TotalAmount;
            order.Notes = orderDto.Notes;

            // Update order items
            order.OrderItems.Clear();
            foreach (var itemDto in orderDto.OrderItems)
            {
                order.OrderItems.Add(new OrderItem
                {
                    ProductId = itemDto.ProductId,
                    Quantity = itemDto.Quantity,
                    UnitPrice = itemDto.UnitPrice,
                    TotalPrice = itemDto.TotalPrice
                });
            }

            await _unitOfWork.SaveChangesAsync();
        }
    }

    public async Task DeleteOrderAsync(int id)
    {
        var order = await _orderRepository.GetByIdAsync(id);
        if (order != null)
        {
            await _orderRepository.DeleteAsync(order);
            await _unitOfWork.SaveChangesAsync();
        }
    }

    private static SalesOrderDto MapToDto(SalesOrder order)
    {
        return new SalesOrderDto
        {
            Id = order.Id,
            OrderNumber = order.OrderNumber,
            OrderDate = order.OrderDate,
            CustomerId = order.CustomerId,
            CustomerName = order.Customer?.Name ?? string.Empty,
            Status = order.Status,
            TotalAmount = order.TotalAmount,
            Notes = order.Notes,
            OrderItems = order.OrderItems.Select(i => new OrderItemDto
            {
                Id = i.Id,
                ProductId = i.ProductId,
                ProductName = i.Product?.Name ?? string.Empty,
                Quantity = i.Quantity,
                UnitPrice = i.UnitPrice,
                TotalPrice = i.TotalPrice
            }).ToList()
        };
    }
}
