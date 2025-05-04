using SimpleEventFlow.Core;

namespace SimpleEventFlow.Tests.ReadmeExampleTests;

public class AccountCreatedEvent : Event
{
    public decimal InitialBalance { get; init; }
}

public class DepositMadeEvent : Event
{
    public decimal Amount { get; init; }
}

public class WithdrawalMadeEvent : Event
{
    public decimal Amount { get; init; }
}