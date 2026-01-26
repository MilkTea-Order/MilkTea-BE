using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using MilkTea.Domain.SharedKernel.Abstractions;

namespace MilkTea.Infrastructure.Persistence.Interceptors;

/// <summary>
/// Tự động publish Domain Events qua MediatR SAU KHI SaveChanges() thành công.
/// 
/// Flow:
/// 1. Handler: order.AddDomainEvent(new OrderCreatedDomainEvent(this))
/// 2. Handler: unitOfWork.SaveChangesAsync()
/// 3. EF Core: Save vào DB (thành công)
/// 4. Interceptor: Lấy DomainEvents từ aggregates → Publish qua MediatR
/// 5. Handlers: Tự động chạy (update inventory, send notification, v.v.)
/// 
/// Lợi ích:
/// - Tự động publish, không cần gọi thủ công
/// - Events chỉ publish SAU KHI save thành công (tránh publish khi rollback)
/// - Tách biệt Domain logic khỏi Infrastructure
/// </summary>
public sealed class DispatchDomainEventsInterceptor(IMediator mediator) : SaveChangesInterceptor
{
    // We buffer events in SavingChanges and publish them only after SaveChanges succeeds.
    // This avoids side-effects running when SaveChanges fails/rolls back.
    private readonly List<IDomainEvent> _pendingEvents = new();

    public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
    {
        CollectDomainEvents(eventData.Context);
        return base.SavingChanges(eventData, result);
    }

    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken cancellationToken = default)
    {
        CollectDomainEvents(eventData.Context);
        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    public override int SavedChanges(SaveChangesCompletedEventData eventData, int result)
    {
        PublishPendingEvents().GetAwaiter().GetResult();
        return base.SavedChanges(eventData, result);
    }

    public override async ValueTask<int> SavedChangesAsync(
        SaveChangesCompletedEventData eventData,
        int result,
        CancellationToken cancellationToken = default)
    {
        await PublishPendingEvents(cancellationToken);
        return await base.SavedChangesAsync(eventData, result, cancellationToken);
    }

    public override void SaveChangesFailed(DbContextErrorEventData eventData)
    {
        _pendingEvents.Clear();
        base.SaveChangesFailed(eventData);
    }

    public override Task SaveChangesFailedAsync(DbContextErrorEventData eventData, CancellationToken cancellationToken = default)
    {
        _pendingEvents.Clear();
        return base.SaveChangesFailedAsync(eventData, cancellationToken);
    }

    private void CollectDomainEvents(DbContext? context)
    {
        if (context == null) return;

        // Tìm tất cả aggregates có DomainEvents
        var aggregates = context.ChangeTracker
            .Entries<IAggregate>()
            .Where(a => a.Entity.DomainEvents.Any())
            .Select(a => a.Entity)
            .ToList();

        var domainEvents = aggregates.SelectMany(a => a.DomainEvents).ToList();
        aggregates.ForEach(a => a.ClearDomainEvents());

        _pendingEvents.AddRange(domainEvents);
    }

    private Task PublishPendingEvents(CancellationToken cancellationToken = default)
    {
        if (_pendingEvents.Count == 0) return Task.CompletedTask;

        // Copy then clear to avoid re-entrancy
        var events = _pendingEvents.ToArray();
        _pendingEvents.Clear();

        return PublishAll(events, cancellationToken);
    }

    private async Task PublishAll(IEnumerable<IDomainEvent> events, CancellationToken cancellationToken)
    {
        foreach (var domainEvent in events)
        {
            await mediator.Publish(domainEvent, cancellationToken);
        }
    }
}
