using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;
using CaseStudy.Application;
using CaseStudy.Application.Common;
using ErrorOr;

namespace CaseStudy.Api.Controllers;

[Route("api/contactpersons")]
[ApiController]
[Authorize]
public class ContactPersonsController(IContactPersonService contactPersonService, IOutputCacheStore cacheStore) : ControllerBase
{
    [HttpGet]
    [OutputCache(PolicyName = "TagById")]
    public async Task<ActionResult<IEnumerable<ContactPersonDto>>> GetAll()
    {
        var contactPersons = await contactPersonService.GetAllAsync();
        return Ok(contactPersons);
    }
        
    [HttpGet("{id}")]
    [OutputCache(PolicyName = "TagById")]
    public async Task<ActionResult<ContactPersonDto>> GetById(Guid id)
    {
        var contactPerson = await contactPersonService.GetByIdAsync(id);
        if (contactPerson == null)
            return NotFound();
        return Ok(contactPerson);
    }
        
    [HttpPost]
    [Authorize(Policy = "Admin")]
    public async Task<IActionResult> Create([FromBody] CreateContactPersonDto dto)
    {
        var newContactPersonId = await contactPersonService.AddAsync(dto);
        await cacheStore.EvictByTagAsync("ContactPersons-All", HttpContext.RequestAborted);
        return CreatedAtAction(nameof(GetById), new { id = newContactPersonId }, dto);
    }
        
    [HttpPut("{id}")]
    [Authorize(Policy = "Admin")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateContactPersonDto dto)
    {
        var updateResult = await contactPersonService.UpdateAsync(id, dto);
        if (updateResult.IsError)
        {
            if (updateResult.FirstError == Error.NotFound())
            {
                return NotFound(updateResult.FirstError.Code);
            }
            return BadRequest(updateResult.FirstError);
        }
        await cacheStore.EvictByTagAsync("ContactPersons-All", HttpContext.RequestAborted);
        await cacheStore.EvictByTagAsync($"TagById-ContactPersons-{id}", HttpContext.RequestAborted);
        return NoContent();
    }
        
    [HttpDelete("{id}")]
    [Authorize(Policy = "Admin")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await contactPersonService.DeleteAsync(id);
        await cacheStore.EvictByTagAsync("ContactPersons-All", HttpContext.RequestAborted);
        await cacheStore.EvictByTagAsync($"TagById-ContactPersons-{id}", HttpContext.RequestAborted);
        return NoContent();
    }
}