namespace SimpleEventFlow.Core;

public interface IEventStore
{
    Task<int> SaveEventsAsync(Guid aggregateId, List<Event> events);
    Task<List<Event>> GetEventsAsync(Guid aggregateId);
}