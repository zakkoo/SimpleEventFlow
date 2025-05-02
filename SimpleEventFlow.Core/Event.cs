namespace SimpleEventFlow.Core;

public record Event
{
    public required Guid EventId {get; init;}
    public required string EventType { get; init; }
    public required int EventTypeVersion { get; init; }
    public required DateTime EventOccuredUtc { get; init; }
    public required Guid AggregateId { get; init; }
    public required int AggregateVersion { get; init; }
    public required string Data { get; init; }
}
