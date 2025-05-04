using SimpleEventFlow.Core;
using SimpleEventFlow.InMemory;

namespace SimpleEventFlow.Tests.ReadmeExampleTests;

public class ExampleTests
{
    [Fact]
    public async Task Readme_StepByStepExample()
    {
        var eventStore = new EventStore();
        var repository = new AggregateRepository(eventStore);

        var accountId = Guid.NewGuid();

        // Create account
        var account = new BankAccount();
        account.CreateAccount(accountId, 100m);
        await repository.SaveAggregateAsync(account);

        // Assert
        Assert.Empty(account.GetUncommittedEvents());

        // Load account and make a deposit
        var loadedAccount = await repository.GetAggregateAsync<BankAccount>(accountId);
        loadedAccount.Deposit(50m);
        await repository.SaveAggregateAsync(loadedAccount);

        // Assert
        Assert.Equal(150m, loadedAccount.Balance);

        // Load account and make a withdrawal
        loadedAccount = await repository.GetAggregateAsync<BankAccount>(accountId);
        loadedAccount.Withdraw(30m);
        await repository.SaveAggregateAsync(loadedAccount);

        // Assert
        Assert.Equal(120m, loadedAccount.Balance);
    }
}