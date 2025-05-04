namespace SimpleEventFlow.Core;

public interface IAggregateRepository
{
    Task<T> GetAggregateAsync<T>(Guid aggregateId) where T : IAggregateRoot, new();
    Task SaveAggregateAsync<T>(T aggregate) where T : IAggregateRoot;
}