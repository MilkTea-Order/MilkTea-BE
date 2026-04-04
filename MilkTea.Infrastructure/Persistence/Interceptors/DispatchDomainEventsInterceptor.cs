using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using MilkTea.Domain.Common.Abstractions;

namespace MilkTea.Infrastructure.Persistence.Interceptors;


public sealed class DispatchDomainEventsInterceptor(IMediator mediator) : SaveChangesInterceptor
{

    private readonly List<IDomainEvent> _vPendingEvents = new();

    public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
    {
        CollectDomainEvents(eventData.Context);
        return base.SavingChanges(eventData, result);
    }

    public override async ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData,
                                                                            InterceptionResult<int> result,
                                                                            CancellationToken cancellationToken = default)
    {
        Console.WriteLine("SavingChangesAsync");
        CollectDomainEvents(eventData.Context);
        await PublishPendingEvents(cancellationToken);
        return await base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    public override int SavedChanges(SaveChangesCompletedEventData eventData, int result)
    {

        //PublishPendingEvents().GetAwaiter().GetResult();
        return base.SavedChanges(eventData, result);
    }

    public override async ValueTask<int> SavedChangesAsync(SaveChangesCompletedEventData eventData,
                                                            int result,
                                                            CancellationToken cancellationToken = default)
    {
        Console.WriteLine("SavedChangesAsync");
        //await PublishPendingEvents(cancellationToken);
        return await base.SavedChangesAsync(eventData, result, cancellationToken);
    }

    public override void SaveChangesFailed(DbContextErrorEventData eventData)
    {
        _vPendingEvents.Clear();
        base.SaveChangesFailed(eventData);
    }

    public override Task SaveChangesFailedAsync(DbContextErrorEventData eventData, CancellationToken cancellationToken = default)
    {
        Console.WriteLine("SaveChangesFailedAsync");
        _vPendingEvents.Clear();
        return base.SaveChangesFailedAsync(eventData, cancellationToken);
    }

    private void CollectDomainEvents(DbContext? context)
    {
        Console.WriteLine("CollectDomainEvents");
        if (context == null) return;

        var aggregates = context.ChangeTracker.Entries<IAggregate>()
                                                .Where(a => a.Entity.DomainEvents.Any())
                                                .Select(a => a.Entity)
                                                .ToList();

        var domainEvents = aggregates.SelectMany(a => a.DomainEvents).ToList();
        aggregates.ForEach(a => a.ClearDomainEvents());

        _vPendingEvents.AddRange(domainEvents);
    }

    private Task PublishPendingEvents(CancellationToken cancellationToken = default)
    {
        Console.WriteLine("PublishPendingEvents");
        if (_vPendingEvents.Count == 0) return Task.CompletedTask;
        var events = _vPendingEvents.ToArray();
        _vPendingEvents.Clear();

        return PublishAll(events, cancellationToken);
    }

    private async Task PublishAll(IEnumerable<IDomainEvent> events, CancellationToken cancellationToken)
    {
        Console.WriteLine("PublishAll");
        foreach (var domainEvent in events)
        {
            await mediator.Publish(domainEvent, cancellationToken);
        }
    }
}
