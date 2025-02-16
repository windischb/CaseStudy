using CaseStudy.Application;
using CaseStudy.Application.Common;
using ErrorOr;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;

namespace CaseStudy.Api.Controllers;

[Route("api/bankaccounts")]
[ApiController]
[Authorize]
public class BankAccountsController(IBankAccountService bankAccountService, IOutputCacheStore cacheStore) : ControllerBase
{
    [HttpGet]
    [OutputCache(PolicyName = "TagById")]
    public async Task<ActionResult<IEnumerable<BankAccountDto>>> GetAll()
    {
        var bankAccounts = await bankAccountService.GetAllAsync();
        return Ok(bankAccounts);
    }
        
    [HttpGet("{id}")]
    [OutputCache(PolicyName = "TagById")]
    public async Task<ActionResult<BankAccountDto>> GetById(Guid id)
    {
        var bankAccount = await bankAccountService.GetByIdAsync(id);
        if (bankAccount == null)
            return NotFound();
        return Ok(bankAccount);
    }
        
    [HttpPost]
    [Authorize(Policy = "Admin")]
    public async Task<IActionResult> Create([FromBody] CreateBankAccountDto dto)
    {
        var newBankAccountId = await bankAccountService.AddAsync(dto);
        await cacheStore.EvictByTagAsync("BankAccounts-All", HttpContext.RequestAborted);
        return CreatedAtAction(nameof(GetById), new { id = newBankAccountId }, dto);
    }
        
    [HttpPut("{id}")]
    [Authorize(Policy = "Admin")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateBankAccountDto dto)
    {
        var updateResult = await bankAccountService.UpdateAsync(id, dto);
        if (updateResult.IsError)
        {
            if (updateResult.FirstError == Error.NotFound())
            {
                return NotFound(updateResult.FirstError.Code);
            }
            return BadRequest(updateResult.FirstError);
        }
        await cacheStore.EvictByTagAsync("BankAccounts-All", HttpContext.RequestAborted);
        await cacheStore.EvictByTagAsync($"TagById-BankAccounts-{id}", HttpContext.RequestAborted);
        return NoContent();
    }
        
    [HttpDelete("{id}")]
    [Authorize(Policy = "Admin")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await bankAccountService.DeleteAsync(id);
        await cacheStore.EvictByTagAsync("BankAccounts-All", HttpContext.RequestAborted);
        await cacheStore.EvictByTagAsync($"TagById-BankAccounts-{id}", HttpContext.RequestAborted);
        return NoContent();
    }
}