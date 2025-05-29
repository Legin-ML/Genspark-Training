using Microsoft.AspNetCore.Mvc;
using LegitBank.Interfaces;
using LegitBank.Models;
using LegitBank.Models.Dtos;

namespace LegitBank.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AccountController : ControllerBase
{
    private readonly IAccountService _accountService;

    public AccountController(IAccountService accountService)
    {
        _accountService = accountService;
    }
    
    [HttpPost]
    public async Task<IActionResult> CreateAccount([FromBody] AccountCreateDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var account = await _accountService.CreateAccount(dto);
        return CreatedAtAction(nameof(GetAccount), new { accountId = account.AccountId }, account);
    }
    
    [HttpGet("{accountId}")]
    public async Task<IActionResult> GetAccount(int accountId)
    {
        var account = await _accountService.GetAccount(accountId);
        if (account == null)
            return NotFound();

        return Ok(account);
    }
    
    [HttpDelete("{accountId}")]
    public async Task<IActionResult> DeleteAccount(int accountId)
    {
        var deleted = await _accountService.DeleteAccount(accountId);
        if (deleted == null)
            return NotFound();

        return Ok(deleted);
    }
}