using Microsoft.EntityFrameworkCore;
using SalesOrderDemo.Application.DTOs;
using SalesOrderDemo.Application.Interfaces;
using SalesOrderDemo.Domain.Enums;
using SalesOrderDemo.Infrastructure.Data;

namespace SalesOrderDemo.Infrastructure.Services;

public class DashboardService : IDashboardService
{
    private readonly ApplicationDbContext _context;

    public DashboardService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<DashboardDto> GetDashboardDataAsync()
    {
        var totalOrders = await _context.SalesOrders.CountAsync();
        var totalCustomers = await _context.Customers.CountAsync();
        var totalProducts = await _context.Products.CountAsync();
        var totalRevenue = await _context.SalesOrders.SumAsync(o => (decimal?)o.TotalAmount) ?? 0;

        var pendingOrders = await _context.SalesOrders.CountAsync(o => o.Status == OrderStatus.Pending);
        var processingOrders = await _context.SalesOrders.CountAsync(o => o.Status == OrderStatus.Processing);
        var shippedOrders = await _context.SalesOrders.CountAsync(o => o.Status == OrderStatus.Shipped);
        var deliveredOrders = await _context.SalesOrders.CountAsync(o => o.Status == OrderStatus.Delivered);

        var recentOrders = await _context.SalesOrders
            .Include(o => o.Customer)
            .OrderByDescending(o => o.OrderDate)
            .Take(10)
            .Select(o => new RecentOrderDto
            {
                Id = o.Id,
                OrderNumber = o.OrderNumber,
                CustomerName = o.Customer.Name,
                OrderDate = o.OrderDate,
                TotalAmount = o.TotalAmount,
                Status = o.Status.ToString()
            })
            .ToListAsync();

        return new DashboardDto
        {
            TotalOrders = totalOrders,
            TotalCustomers = totalCustomers,
            TotalProducts = totalProducts,
            TotalRevenue = totalRevenue,
            PendingOrders = pendingOrders,
            ProcessingOrders = processingOrders,
            ShippedOrders = shippedOrders,
            DeliveredOrders = deliveredOrders,
            RecentOrders = recentOrders
        };
    }
}
