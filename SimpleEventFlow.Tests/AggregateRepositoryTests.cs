using SimpleEventFlow.Core;
using SimpleEventFlow.InMemory;

namespace SimpleEventFlow.Tests;

public class AggregateRepositoryTests
{
    private readonly AggregateRepository _repository;
    private readonly EventStore _eventStore;

    public AggregateRepositoryTests()
    {
        _eventStore = new EventStore();
        _repository = new AggregateRepository(_eventStore);
    }

    [Fact]
    public async Task GetAggregateAsync_WhenNoEvents_ReturnsNewAggregate()
    {
        // Arrange
        var aggregateId = Guid.NewGuid();

        // Act
        var aggregate = await _repository.GetAggregateAsync<TestAggregate>(aggregateId);

        // Assert
        Assert.NotNull(aggregate);
        Assert.Equal(aggregateId, aggregate.AggregateId);
        Assert.Equal(0, aggregate.AggregateVersion);
        Assert.Empty(aggregate.GetUncommittedEvents());
    }

    [Fact]
    public async Task GetAggregateAsync_WhenEventsExist_ReplaysEventsCorrectly()
    {
        // Arrange
        var aggregateId = Guid.NewGuid();
        var event1 = new TestEvent
        {
            EventId = Guid.NewGuid(),
            EventType = nameof(TestEvent),
            EventTypeVersion = 1,
            EventOccuredUtc = DateTime.UtcNow,
            AggregateId = aggregateId,
            AggregateVersion = 1,
            Data = "Test1"
        };
        var event2 = new TestEvent
        {
            EventId = Guid.NewGuid(),
            EventType = nameof(TestEvent),
            EventTypeVersion = 1,
            EventOccuredUtc = DateTime.UtcNow,
            AggregateId = aggregateId,
            AggregateVersion = 2,
            Data = "Test2"
        };
        await _eventStore.SaveEventsAsync(aggregateId, new List<Event> { event1, event2 });

        // Act
        var aggregate = await _repository.GetAggregateAsync<TestAggregate>(aggregateId);

        // Assert
        Assert.Equal(aggregateId, aggregate.AggregateId);
        Assert.Equal(2, aggregate.AggregateVersion);
        Assert.Equal("Test2", aggregate.State);
    }

    [Fact]
    public async Task SaveAggregateAsync_WhenNoUncommittedEvents_DoesNotSave()
    {
        // Arrange
        var aggregateId = Guid.NewGuid();
        var aggregate = new TestAggregate();
        aggregate.SetAggregateId(aggregateId);

        // Act
        await _repository.SaveAggregateAsync(aggregate);

        // Assert
        var events = await _eventStore.GetEventsAsync(aggregateId);
        Assert.Empty(events);
    }

    [Fact]
    public async Task SaveAggregateAsync_WhenUncommittedEvents_SavesAndClearsEvents()
    {
        // Arrange
        var aggregateId = Guid.NewGuid();
        var aggregate = new TestAggregate();
        aggregate.SetAggregateId(aggregateId);
        aggregate.Create(aggregateId, "TestData");

        // Act
        await _repository.SaveAggregateAsync(aggregate);

        // Assert
        var events = await _eventStore.GetEventsAsync(aggregateId);
        Assert.Single(events);
        Assert.Empty(aggregate.GetUncommittedEvents());
        Assert.Equal("TestData", ((TestEvent)events[0]).Data);
    }

    [Fact]
    public async Task SaveAggregateAsync_WhenAggregateIdIsNull_ThrowsNullReferenceException()
    {
        // Arrange
        var aggregate = new TestAggregate();

        // Act & Assert
        await Assert.ThrowsAsync<NullReferenceException>(() => _repository.SaveAggregateAsync(aggregate));
    }
}