using CaseStudy.Application.Interfaces;
using CaseStudy.Domain;
using Microsoft.AspNetCore.Mvc;

namespace CaseStudy.Api.Controllers;

[Route("api/contactpersons")]
[ApiController]
public class ContactPersonsController(IContactPersonRepository contactPersonRepository) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<ContactPerson>>> GetAll()
    {
        var contactPersons = await contactPersonRepository.GetAllAsync();
        return Ok(contactPersons);
    }
        
    [HttpGet("{id}")]
    public async Task<ActionResult<ContactPerson>> GetById(Guid id)
    {
        var contactPerson = await contactPersonRepository.GetByIdAsync(id);
        if (contactPerson == null)
            return NotFound();
        return Ok(contactPerson);
    }
        
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] ContactPerson vendor)
    {
        await contactPersonRepository.AddAsync(vendor);
        return CreatedAtAction(nameof(GetById), new { id = vendor.Id }, vendor);
    }
        
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] ContactPerson vendor)
    {
        if (id != vendor.Id)
            return BadRequest();
            
        await contactPersonRepository.UpdateAsync(vendor);
        return NoContent();
    }
        
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await contactPersonRepository.DeleteAsync(id);
        return NoContent();
    }
}