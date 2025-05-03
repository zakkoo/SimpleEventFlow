using SimpleEventFlow.Core;

namespace SimpleEventFlow.Tests;

public record OrderCanceledEvent(
    Guid EventId,
    string EventType,
    int EventTypeVersion,
    DateTime EventOccuredUtc,
    Guid AggregateId,
    int AggregateVersion
) :
Event(EventId, EventType, EventTypeVersion, EventOccuredUtc, AggregateId, AggregateVersion);
