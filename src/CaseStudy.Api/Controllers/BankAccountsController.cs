using CaseStudy.Application.Interfaces;
using CaseStudy.Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;

namespace CaseStudy.Api.Controllers;

[Route("api/bankaccounts")]
[ApiController]
[Authorize]
public class BankAccountsController(IBankAccountRepository bankAccountRepository, IOutputCacheStore cacheStore) : ControllerBase
{
    [HttpGet]
    [OutputCache(PolicyName = "TagById")]
    public async Task<ActionResult<IEnumerable<BankAccount>>> GetAll()
    {
        var bankAccounts = await bankAccountRepository.GetAllAsync();
        return Ok(bankAccounts);
    }
        
    [HttpGet("{id}")]
    [OutputCache(PolicyName = "TagById")]
    public async Task<ActionResult<BankAccount>> GetById(Guid id)
    {
        var bankAccount = await bankAccountRepository.GetByIdAsync(id);
        if (bankAccount == null)
            return NotFound();
        return Ok(bankAccount);
    }
        
    [HttpPost]
    [Authorize(Policy = "Admin")]
    public async Task<IActionResult> Create([FromBody] BankAccount vendor)
    {
        await bankAccountRepository.AddAsync(vendor);
        await cacheStore.EvictByTagAsync("BankAccounts-All", this.HttpContext.RequestAborted);
        return CreatedAtAction(nameof(GetById), new { id = vendor.Id }, vendor);
    }
        
    [HttpPut("{id}")]
    [Authorize(Policy = "Admin")]
    public async Task<IActionResult> Update(Guid id, [FromBody] BankAccount vendor)
    {
        if (id != vendor.Id)
            return BadRequest();
            
        await bankAccountRepository.UpdateAsync(vendor);
        await cacheStore.EvictByTagAsync("BankAccounts-All", this.HttpContext.RequestAborted);
        await cacheStore.EvictByTagAsync($"TagById-BankAccounts-{id}", this.HttpContext.RequestAborted);
        return NoContent();
    }
        
    [HttpDelete("{id}")]
    [Authorize(Policy = "Admin")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await bankAccountRepository.DeleteAsync(id);
        await cacheStore.EvictByTagAsync("BankAccounts-All", this.HttpContext.RequestAborted);
        await cacheStore.EvictByTagAsync($"TagById-BankAccounts-{id}", this.HttpContext.RequestAborted);
        return NoContent();
    }
}