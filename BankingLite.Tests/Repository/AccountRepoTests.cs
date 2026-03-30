using Xunit;
using BankingLite.Api.Repository;
using BankingLite.Api.Data;
using BankingLite.Api.Models.Entities;
using BankingLite.Api.Models.DTOs.Request;
using Microsoft.EntityFrameworkCore;

public class AccountRepoTests
{
    private BankingLiteDbContext GetDbContext()
    {
        var options = new DbContextOptionsBuilder<BankingLiteDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        return new BankingLiteDbContext(options);
    }

    [Fact]
    public async Task CreateAccount_Should_Work()
    {
        var context = GetDbContext();
        var repo = new AccountRepo(context);

        var request = new AccountCreate
        {
            UserId = 1,
            BankName = "GTBank",
            AccountNumber = "123",
            Balance = 1000
        };

        var result = await repo.CreateAccountAsync(request);

        Assert.NotNull(result);
        Assert.Equal("GTBank", result.BankName);
    }
}