using BankingLite.Api.Interface;
using BankingLite.Api.Models.DTOs.Request;
using BankingLite.Api.Models.DTOs.Response;
using Microsoft.AspNetCore.Mvc;

namespace BankingLite.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountRepo _accountRepo;

        public AccountController(IAccountRepo accountRepo)
        {
            _accountRepo = accountRepo;
        }

        [HttpPost("create-account")]
        public async Task<ActionResult<AccountRead>> CreateAccount(AccountCreate request)
        {
            try
            {
                ArgumentNullException.ThrowIfNull(request, nameof(request));
                if (string.IsNullOrWhiteSpace(request.BankName) || string.IsNullOrWhiteSpace(request.AccountNumber))
                    return BadRequest("BankName and AccountNumber are required.");
                var createdAccount = await _accountRepo.CreateAccountAsync(request);
                return CreatedAtAction(nameof(GetAccountById), new { accountId = createdAccount.AccountId }, createdAccount);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("{accountId}")]
        public async Task<ActionResult<AccountRead>> GetAccountById(int accountId)
        {
            var account = await _accountRepo.GetAccountByIdAsync(accountId);
            if (account == null) return NotFound();
            return Ok(account);
        }

        [HttpGet("user/{userId}")]
        public async Task<ActionResult<IEnumerable<AccountRead>>> GetAccountsByUserId(int userId)
        {
            try
            {
                var accounts = await _accountRepo.GetAccountsByUserIdAsync(userId);
                if (accounts == null || !accounts.Any()) return NotFound();
                return Ok(accounts);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPut("update-account/{accountId}")]
        public async Task<ActionResult<AccountRead>> UpdateAccount(int accountId, AccountCreate request)
        {
            ArgumentNullException.ThrowIfNull(request, nameof(request));

            if (string.IsNullOrWhiteSpace(request.BankName) || string.IsNullOrWhiteSpace(request.AccountNumber))
                return BadRequest("BankName and AccountNumber are required.");

            try
            {
                var updatedAccount = await _accountRepo.UpdateAccountAsync(request, accountId);
                return Ok(updatedAccount);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}