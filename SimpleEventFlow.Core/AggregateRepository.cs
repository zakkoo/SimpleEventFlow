
namespace SimpleEventFlow.Core;

public class AggregateRepository : IAggregateRepository
{
    private readonly IEventStore _eventStore;

    public AggregateRepository(IEventStore eventStore)
    {
        _eventStore = eventStore;
    }

    public async Task<T> GetAggregateAsync<T>(Guid aggregateId) where T : IAggregateRoot, new()
    {
        var events = await _eventStore.GetEventsAsync(aggregateId);
        var aggregate = new T();
        aggregate.SetAggregateId(aggregateId);
        foreach (var evt in events)
        {
            aggregate.ApplyEvent(evt);
        }
        return aggregate;
    }

    public async Task SaveAggregateAsync<T>(T aggregate) where T : IAggregateRoot
    {
        var events = aggregate.GetUncommittedEvents();
        if(events.Count == 0) return;
        await _eventStore.SaveEventsAsync(aggregate.AggregateId ?? throw new NullReferenceException(nameof(aggregate.AggregateId)), events);
        aggregate.ClearUncommittedEvents();
    }
}