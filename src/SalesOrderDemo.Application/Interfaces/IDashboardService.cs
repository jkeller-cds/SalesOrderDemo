using SalesOrderDemo.Application.DTOs;

namespace SalesOrderDemo.Application.Interfaces;

public interface IDashboardService
{
    Task<DashboardDto> GetDashboardDataAsync();
}
