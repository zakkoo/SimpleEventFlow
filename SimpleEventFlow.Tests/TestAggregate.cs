using SimpleEventFlow.Core;
using SimpleEventFlow.Tests;

public class TestAggregate : AggregateRoot
{
    public string State { get; private set; } = string.Empty;

    public TestAggregate()
    {
        RegisterHandler<TestEvent>(Handle);
    }

    private void Handle(TestEvent evt)
    {
        State = evt.Data;
    }

    public void Create(Guid id, string data)
    {
        var evt = new TestEvent
        {
            EventId = Guid.NewGuid(),
            EventType = nameof(TestEvent),
            EventTypeVersion = 1,
            EventOccuredUtc = DateTime.UtcNow,
            AggregateId = id,
            AggregateVersion = AggregateVersion + 1,
            Data = data
        };
        RaiseEvent(evt);
    }
}