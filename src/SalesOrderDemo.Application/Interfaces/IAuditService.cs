namespace SalesOrderDemo.Application.Interfaces;

public interface IAuditService
{
    Task LogAuditAsync(string entityName, string action, int entityId, string changes, string userId);
}
