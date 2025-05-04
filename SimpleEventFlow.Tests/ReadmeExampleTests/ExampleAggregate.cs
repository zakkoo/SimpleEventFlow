using SimpleEventFlow.Core;

namespace SimpleEventFlow.Tests.ReadmeExampleTests;

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