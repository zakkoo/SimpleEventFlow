using SimpleEventFlow.Core;

namespace SimpleEventFlow.Tests;

public class OrderCreatedEvent : Event
{
    public required Guid CustomerId { get; init; }
    public required string CustomerEmail { get; init; }
    public required string ShoppingItem { get; init; }
}