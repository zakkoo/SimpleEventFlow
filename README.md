# SimpleEventFlow

A simple implementation of the event sourcing pattern in C#.

## Description

This project demonstrates the basics of event sourcing, where the state of an application is determined by a sequence of events. It includes:

- **Event**: A base class for all events.
- **AggregateRoot**: An abstract class for aggregate roots, which are entities that handle events and maintain state.
- **EventStore**: An in-memory implementation of an event store to persist events.
- **AggregateRepository**: A repository to load and save aggregates by applying events.

## Features

- Event sourcing pattern implementation.
- In-memory event store for simplicity.
- Easy-to-use aggregate repository for managing aggregates.

## Usage

1. **Define Events**: Create event classes by inheriting from `Event`.
2. **Define Aggregates**: Create aggregate classes by inheriting from `AggregateRoot`. Register event handlers and raise events when state changes.
3. **Use Repository**: Use `AggregateRepository` to load and save your aggregates.

## Example

Here's a simple example of a bank account using event sourcing.

### Events

```csharp
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
```

### Aggregate

```csharp
public class BankAccount : AggregateRoot
{
    public decimal Balance { get; private set; }

    public BankAccount()
    {
        RegisterHandler<AccountCreatedEvent>(Handle);
        RegisterHandler<DepositMadeEvent>(Handle);
        RegisterHandler<WithdrawalMadeEvent>(Handle);
    }

    private void Handle(AccountCreatedEvent evt)
    {
        Balance = evt.InitialBalance;
    }

    private void Handle(DepositMadeEvent evt)
    {
        Balance += evt.Amount;
    }

    private void Handle(WithdrawalMadeEvent evt)
    {
        Balance -= evt.Amount;
    }

    public void CreateAccount(Guid accountId, decimal initialBalance)
    {
        AggregateId = accountId;
        
        var evt = new AccountCreatedEvent
        {
            EventId = Guid.NewGuid(),
            EventType = nameof(AccountCreatedEvent),
            EventTypeVersion = 1,
            EventOccuredUtc = DateTime.UtcNow,
            AggregateId = accountId,
            AggregateVersion = this.AggregateVersion + 1,
            InitialBalance = initialBalance
        };
        RaiseEvent(evt);
    }

    public void Deposit(decimal amount)
    {
        var evt = new DepositMadeEvent
        {
            EventId = Guid.NewGuid(),
            EventType = nameof(DepositMadeEvent),
            EventTypeVersion = 1,
            EventOccuredUtc = DateTime.UtcNow,
            AggregateId = this.AggregateId ?? throw new InvalidOperationException("AggregateId not set"),
            AggregateVersion = this.AggregateVersion + 1,
            Amount = amount
        };
        RaiseEvent(evt);
    }

    public void Withdraw(decimal amount)
    {
        if (Balance < amount)
            throw new InvalidOperationException("Insufficient funds");

        var evt = new WithdrawalMadeEvent
        {
            EventId = Guid.NewGuid(),
            EventType = nameof(WithdrawalMadeEvent),
            EventTypeVersion = 1,
            EventOccuredUtc = DateTime.UtcNow,
            AggregateId = this.AggregateId ?? throw new InvalidOperationException("AggregateId not set"),
            AggregateVersion = this.AggregateVersion + 1,
            Amount = amount
        };
        RaiseEvent(evt);
    }
}
```

### Using the Repository

```csharp
var eventStore = new EventStore();
var repository = new AggregateRepository(eventStore);

var accountId = Guid.NewGuid();

// Create account
var account = new BankAccount();
account.CreateAccount(accountId, 100m);
await repository.SaveAggregateAsync(account);

// Load account and make a deposit
var loadedAccount = await repository.GetAggregateAsync<BankAccount>(accountId);
loadedAccount.Deposit(50m);
await repository.SaveAggregateAsync(loadedAccount);

// Load account and make a withdrawal
loadedAccount = await repository.GetAggregateAsync<BankAccount>(accountId);
loadedAccount.Withdraw(30m);
await repository.SaveAggregateAsync(loadedAccount);
```

## Installation

...

## Notes

- This implementation uses an in-memory event store for simplicity. In a production environment, you would want to use a persistent event store.
- Concurrency control and other advanced features are not implemented in this example.
