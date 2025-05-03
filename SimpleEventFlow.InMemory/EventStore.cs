using SimpleEventFlow.Core;

namespace SimpleEventFlow.InMemory;

public class EventStore : IEventStore
{
    private readonly Dictionary<Guid, List<Event>> _events = [];

    public Task<List<Event>> GetEventsAsync(Guid aggregateId)
    {
        if (_events.TryGetValue(aggregateId, out var events))
        {
            return Task.FromResult(events.OrderBy(e => e.AggregateVersion).ToList());
        }
        return Task.FromResult(new List<Event>());
    }

    public Task<int> SaveEventsAsync(Guid aggregateId, List<Event> events)
    {
        if (!_events.TryGetValue(aggregateId, out List<Event>? existingEvents))
        {
            existingEvents = [];
            _events[aggregateId] = existingEvents;
        }

        existingEvents.AddRange(events);
        return Task.FromResult(events.Count);
    }
}