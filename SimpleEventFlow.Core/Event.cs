namespace SimpleEventFlow.Core;

public abstract record class Event (
    Guid EventId,
    string EventType,
    int EventTypeVersion,
    DateTime EventOccuredUtc,
    Guid AggregateId,
    int AggregateVersion 
);

