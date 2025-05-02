using System.Reflection.Metadata.Ecma335;
using SimpleEventFlow.Core;

namespace SimpleEventFlow.InMemory;

public class EventStore() : IEventStore
{
    private readonly Dictionary<Guid, List<Event>> _Events = [];
    public Task<List<Event>> GetEventsAsync(Guid aggregateId)
    {
        var result = _Events.TryGetValue(aggregateId, out var events) ?  events.OrderBy(x => x.AggregateVersion).ToList() : [];
        return Task.FromResult(result);
    }

    public Task<int> SaveEventsAsync(Guid aggregateId, List<Event> events)
    {
        var result = _Events.TryAdd(aggregateId, events.OrderBy(x => x.AggregateVersion).ToList()) ? events.Count : 0;
        return Task.FromResult(result);
    }
}