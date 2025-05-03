using SimpleEventFlow.Core;

namespace SimpleEventFlow.Tests;

public record class OrderCreatedEvent(
    Guid EventId,
    string EventType,
    int EventTypeVersion,
    DateTime EventOccuredUtc,
    Guid AggregateId,
    int AggregateVersion,
    Guid CustomerId,
    string CustomerEmail,
    string ShoppingItem
) : Event(EventId, EventType, EventTypeVersion, EventOccuredUtc, AggregateId, AggregateVersion);