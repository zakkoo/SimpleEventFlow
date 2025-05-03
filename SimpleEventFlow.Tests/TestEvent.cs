using SimpleEventFlow.Core;

namespace SimpleEventFlow.Tests;

public class TestEvent : Event
{
    public required string Data { get; init; }
}