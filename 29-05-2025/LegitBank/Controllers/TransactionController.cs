using Microsoft.AspNetCore.Mvc;
using LegitBank.Interfaces;

namespace LegitBank.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TransactionController : ControllerBase
{
    private readonly ITransactionService _transactionService;

    public TransactionController(ITransactionService transactionService)
    {
        _transactionService = transactionService;
    }

    [HttpPost("deposit")]
    public async Task<IActionResult> Deposit(int accountId, decimal amount)
    {
        try
        {
            var result = await _transactionService.Deposit(accountId, amount);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPost("withdraw")]
    public async Task<IActionResult> Withdraw(int accountId, decimal amount)
    {
        try
        {
            var result = await _transactionService.Withdraw(accountId, amount);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPost("send")]
    public async Task<IActionResult> Send(int senderId, int receiverId, decimal amount)
    {
        try
        {
            var result = await _transactionService.Send(senderId, receiverId, amount);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    // [HttpGet("account/{accountId}")]
    // public async Task<IActionResult> ListTransactions(int accountId)
    // {
    //     try
    //     {
    //         var results = await _transactionService.GetTransactionsForAccount(accountId);
    //         return Ok(results);
    //     }
    //     catch (Exception ex)
    //     {
    //         return BadRequest(ex.Message);
    //     }
    // }
}