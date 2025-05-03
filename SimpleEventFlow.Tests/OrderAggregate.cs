using SimpleEventFlow.Core;

namespace SimpleEventFlow.Tests;

public class OrderAggregate : AggregateRoot
{
    public bool IsOrderCanceled { get; private set; }
    public Guid? CustomerId { get; private set; }
    public string? CustomerEmail { get; private set; }
    public string? ShoppingItem { get; private set; }

    public OrderAggregate() : base()
    {
        RegisterHandler<OrderCreatedEvent>(Apply);
        RegisterHandler<OrderCanceledEvent>(Apply);
    }

    public OrderAggregate(Guid aggregateId) : this()
    {
        SetAggregateId(aggregateId);
    }

    private void Apply(OrderCreatedEvent evt)
    {
        CustomerId = evt.CustomerId;
        CustomerEmail = evt.CustomerEmail;
        ShoppingItem = evt.ShoppingItem;
    }

    private void Apply(OrderCanceledEvent evt)
    {
        IsOrderCanceled = true;
    }

    public void CreateOrder(Guid customerId, string customerEmail, string shoppingItem)
    {
        if (AggregateId == null)
            throw new InvalidOperationException("AggregateId must be set.");

        var evt = new OrderCreatedEvent
        (
            Guid.NewGuid(),
            "OrderCreated",
            1,
            DateTime.UtcNow,
            AggregateId.Value,
            AggregateVersion + 1,
            customerId,
            customerEmail,
            shoppingItem
        );
        RaiseEvent(evt);
    }

    public void CancelOrder()
    {
        if (AggregateId == null)
            throw new InvalidOperationException("AggregateId must be set.");

        var evt = new OrderCanceledEvent
        (
            Guid.NewGuid(),
            "OrderCanceled",
            1,
            DateTime.UtcNow,
            AggregateId.Value,
            AggregateVersion + 1
        );
        RaiseEvent(evt);
    }
}