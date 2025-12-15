using SalesOrderDemo.Application.Interfaces;
using SalesOrderDemo.Domain.Entities;
using SalesOrderDemo.Domain.Interfaces;

namespace SalesOrderDemo.Infrastructure.Services;

public class AuditService : IAuditService
{
    private readonly IRepository<AuditLog> _auditRepository;
    private readonly IUnitOfWork _unitOfWork;

    public AuditService(IRepository<AuditLog> auditRepository, IUnitOfWork unitOfWork)
    {
        _auditRepository = auditRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task LogAuditAsync(string entityName, string action, int entityId, string changes, string userId)
    {
        var auditLog = new AuditLog
        {
            EntityName = entityName,
            Action = action,
            EntityId = entityId,
            Changes = changes,
            UserId = userId,
            Timestamp = DateTime.UtcNow
        };

        await _auditRepository.AddAsync(auditLog);
        await _unitOfWork.SaveChangesAsync();
    }
}
