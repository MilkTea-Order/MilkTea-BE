using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using MilkTea.Domain.SharedKernel.Abstractions;

namespace MilkTea.Infrastructure.Persistence.Interceptors;

public sealed class AuditableEntityInterceptor : SaveChangesInterceptor
{
    public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
    {
        UpdateEntities(eventData.Context);
        return base.SavingChanges(eventData, result);
    }

    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken cancellationToken = default)
    {
        UpdateEntities(eventData.Context);
        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    private void UpdateEntities(DbContext? context)
    {
        if (context == null) return;

        // Local Time
        var now = DateTime.Now;

        foreach (var entry in context.ChangeTracker.Entries<IAuditable>())
        {
            //if (entry.State == EntityState.Added)
            //{
            //    // For new entities, set both created and modified timestamps
            //    entry.Entity.CreatedDate = now;
            //}
            //else if (entry.State == EntityState.Modified || entry.HasChangedOwnedEntities())
            //{
            //    entry.Entity.UpdatedDate = now;
            //}
        }
    }
}
