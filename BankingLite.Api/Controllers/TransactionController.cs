using BankingLite.Api.Interface;
using BankingLite.Api.Models.DTOs.Request;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BankingLite.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class TransactionController : ControllerBase
    {
        private readonly ITransactionRepo _transactionRepo;

        public TransactionController(ITransactionRepo transactionRepo)
        {
            _transactionRepo = transactionRepo;
        }
        
        [HttpPost("transfer")]
        public async Task<IActionResult> Transfer([FromBody] TransactionCreate request)
        {
            if (request.Amount <= 0)
                return BadRequest("Amount must be greater than zero.");
            try
            {                
                var result = await _transactionRepo.TransferFundAsync(request);
                return Ok(result);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetTransactionsByUserIdAsync(int userId)
        {
            var history = await _transactionRepo.GetTransactionsByUserIdAsync(userId);
            return Ok(history);
        }       

        [HttpGet("account/{accountId}")]
        public async Task<IActionResult> GetTransactionsByAccountIdAsync(int accountId)
        {
            var history = await _transactionRepo.GetTransactionsByAccountIdAsync(accountId);
            return Ok(history);
        }
    }
}