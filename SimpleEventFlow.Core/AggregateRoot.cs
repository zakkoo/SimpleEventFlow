namespace SimpleEventFlow.Core;

public abstract class AggregateRoot : IAggregateRoot
{
    private readonly Dictionary<Type, Action<Event>> _eventHandlers = [];
    private readonly List<Event> _uncommittedEvents = [];
    public Guid? AggregateId { get; protected set; }
    public int AggregateVersion { get; protected set; }

    public List<Event> GetUncommittedEvents() => _uncommittedEvents;
    public void ClearUncommittedEvents() => _uncommittedEvents.Clear();

    public void ApplyEvent(Event evt)
    {
        if (_eventHandlers.TryGetValue(evt.GetType(), out var handler))
        {
            handler(evt);
            this.AggregateVersion = evt.AggregateVersion;
        }
        else
        {
            throw new InvalidOperationException($"No handler for event type {evt.GetType()}");
        }
    }

    public bool SetAggregateId(Guid aggregateId)
    {
        if (AggregateId != null) return false;
        AggregateId = aggregateId;
        return true;
    }

    protected void RegisterHandler<TEvent>(Action<TEvent> handler) where TEvent : Event
    {
        _eventHandlers[typeof(TEvent)] = evt => handler((TEvent)evt);
    }

    protected void RaiseEvent(Event evt)
    {
        ApplyEvent(evt);
        _uncommittedEvents.Add(evt);
    }
}