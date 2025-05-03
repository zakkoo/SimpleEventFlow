using SimpleEventFlow.Core;
using SimpleEventFlow.InMemory;

namespace SimpleEventFlow.Tests;

public class OrderTests
{
    [Fact]
    public async Task Temporary()
    {
        var eventStore = new EventStore();
        var aggregateRepository = new AggregateRepository(eventStore);
        var orderId = Guid.Parse("2E03096F-C60B-473D-93F8-0782C09516DC");
        var customerId = Guid.Parse("BFF469BB-48CF-439B-AC3D-B899A4D40A56");
        var orderAggregate = new OrderAggregate(orderId);
        orderAggregate.CreateOrder(customerId, "test@email.com", "Gold Membership");
        orderAggregate.CancelOrder();
        await aggregateRepository.SaveAggregateAsync(orderAggregate);
        var loadedOrderAggregate = await aggregateRepository.GetAggregateAsync<OrderAggregate>(orderId);
        Assert.Equal(orderAggregate.AggregateId, loadedOrderAggregate.AggregateId);
        Assert.Equal(orderAggregate.IsOrderCanceled, loadedOrderAggregate.IsOrderCanceled);
        Assert.Equal(orderAggregate.CustomerEmail, loadedOrderAggregate.CustomerEmail);
        Assert.Equal(orderAggregate.ShoppingItem, loadedOrderAggregate.ShoppingItem);
    }
}
