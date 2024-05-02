using EmployeeInterceptor.Api.Entities;
using EmployeeInterceptor.Api.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace EmployeeInterceptor.Api.Data;

public class EmployeeAuditInterceptor : SaveChangesInterceptor
{
    public override async ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken)    
    {
        if (eventData.Context is not null)
        {
            UpdateAuditableEntities(eventData.Context);
        }
        
        return await base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    private async void UpdateAuditableEntities(DbContext eventDataContext)
    {
        var entities = eventDataContext.ChangeTracker.Entries<IAuditable>()
            .Where(e => e.State is not (EntityState.Detached or EntityState.Unchanged))
            .ToList();    
        
        foreach (var entity in entities)
        {
            await AddAuditEntryAsync(entity, eventDataContext);
        }
    }

    private async Task AddAuditEntryAsync(EntityEntry<IAuditable> entity, DbContext eventDataContext)
    {
        DateTime utcNow = DateTime.UtcNow;
    
        var auditEntry = new EmployeeAudit
        {
            EmployeeId = (int)entity.Property(nameof(Employee.Id)).CurrentValue,
            CreatedAt = entity.State == EntityState.Added ? utcNow : null,
            ModifiedAt = entity.State == EntityState.Added  ? null : utcNow,
            Type = entity.State.ToString(),
            FullName = $"{(string)entity.Property(nameof(Employee.FirstName)).CurrentValue} {(string)entity.Property(nameof(Employee.LastName)).CurrentValue}"
        };

        eventDataContext.Set<EmployeeAudit>().Add(auditEntry);    }
}