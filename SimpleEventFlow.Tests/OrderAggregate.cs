using System.Text.Json;
using SimpleEventFlow.Core;

namespace SimpleEventFlow.Tests;

public class OrderAggregate : IAggregateRoot
{
    private readonly List<Event> _UncommittedEvents = [];
    public Guid? AggregateId { get; private set; }
    public int AggregateVersion { get; private set; }
    public bool IsOrderCanceled { get; private set; }
    public Guid? CustomerId { get; private set; }
    public string? CustomerEmail { get; private set; }
    public string? ShoppingItem { get; private set; }

    public OrderAggregate(Guid aggregateId)
    {
        AggregateId = aggregateId;
    }
    public OrderAggregate() { }

    public void ApplyEvent(Event evt)
    {
        switch (evt.EventType)
        {
            case "OrderCreated":
                var data = JsonSerializer.Deserialize<OrderCreatedEventData>(evt.Data) ?? throw new NullReferenceException($"No data found for {evt.EventType}");
                CustomerId = data.CustomerId;
                CustomerEmail = data.CustomerEmail;
                ShoppingItem = data.ShoppingItem;
                break;
            case "OrderCanceled":
                IsOrderCanceled = true;
                break;
        }
        AggregateVersion++;
    }

    public Event CreateOrder(OrderCreatedEventData data)
    {
        var evt = new Event
        {
            EventId = Guid.NewGuid(),
            EventType = "OrderCreated",
            EventTypeVersion = 1,
            EventOccuredUtc = DateTime.UtcNow,
            AggregateId = AggregateId ?? throw new NullReferenceException(nameof(AggregateId)),
            AggregateVersion = AggregateVersion,
            Data = JsonSerializer.Serialize(data)
        };
        ApplyEvent(evt);
        _UncommittedEvents.Add(evt);
        return evt;
    }

    public Event CancelOrder()
    {
        var evt = new Event
        {
            EventId = Guid.NewGuid(),
            EventType = "OrderCanceled",
            EventTypeVersion = 1,
            EventOccuredUtc = DateTime.UtcNow,
            AggregateId = AggregateId ?? throw new NullReferenceException(nameof(AggregateId)),
            AggregateVersion = AggregateVersion,
            Data = string.Empty
        };
        ApplyEvent(evt);
        _UncommittedEvents.Add(evt);
        return evt;
    }

    public void ClearUncommittedEvents()
    {
        _UncommittedEvents.Clear();
    }

    public List<Event> GetUncommittedEvents()
    {
        return _UncommittedEvents;
    }

    public bool SetAggregateId(Guid aggregateId)
    {
        if (AggregateId != null) return false;

        AggregateId = aggregateId;
        return true;
    }
}