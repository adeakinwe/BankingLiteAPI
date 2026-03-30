using Xunit;
using Moq;
using Microsoft.AspNetCore.Mvc;
using BankingLite.Api.Controllers;
using BankingLite.Api.Interface;
using BankingLite.Api.Models.DTOs.Request;
using BankingLite.Api.Models.DTOs.Response;

public class AccountControllerTests
{
    private readonly Mock<IAccountRepo> _mockRepo;
    private readonly AccountController _controller;

    public AccountControllerTests()
    {
        _mockRepo = new Mock<IAccountRepo>();
        _controller = new AccountController(_mockRepo.Object);
    }

    [Fact]
    public async Task GetAccount_Should_Return_NotFound()
    {
        _mockRepo.Setup(r => r.GetAccountByIdAsync(1))
                 .ReturnsAsync((AccountRead?)null);

        var result = await _controller.GetAccountById(1);

        Assert.IsType<NotFoundResult>(result.Result);
    }

    [Fact]
    public async Task CreateAccount_Should_Return_Created()
    {
        var request = new AccountCreate
        {
            UserId = 1,
            BankName = "GTBank",
            AccountNumber = "123",
            Balance = 100
        };

        var response = new AccountRead
        {
            AccountId = 1,
            UserId = 1,
            BankName = "GTBank",
            AccountNumber = "123",
            Balance = 100
        };

        _mockRepo.Setup(r => r.CreateAccountAsync(request))
                 .ReturnsAsync(response);

        var result = await _controller.CreateAccount(request);

        Assert.IsType<CreatedAtActionResult>(result.Result);
    }
}