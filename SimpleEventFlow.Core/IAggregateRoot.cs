namespace SimpleEventFlow.Core;

public interface IAggregateRoot
{
    Guid? AggregateId { get; }
    int AggregateVersion { get; }
    List<Event> GetUncommittedEvents();
    void ClearUncommittedEvents();
    void ApplyEvent(Event evt);
    bool SetAggregateId(Guid aggregateId);
}