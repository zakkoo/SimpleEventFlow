namespace SimpleEventFlow.Core;

public record OrderCreatedEventData(Guid CustomerId, string CustomerEmail, string ShoppingItem);