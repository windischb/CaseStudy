using CaseStudy.Application.Interfaces;
using CaseStudy.Domain;
using Microsoft.AspNetCore.Mvc;

namespace CaseStudy.Api.Controllers;

[Route("api/vendors")]
[ApiController]
public class VendorsController(IVendorRepository vendorRepository) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Vendor>>> GetAll()
    {
        var vendors = await vendorRepository.GetAllAsync();
        return Ok(vendors);
    }
        
    [HttpGet("{id}")]
    public async Task<ActionResult<Vendor>> GetById(Guid id)
    {
        var vendor = await vendorRepository.GetByIdAsync(id);
        if (vendor == null)
            return NotFound();
        return Ok(vendor);
    }
        
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] Vendor vendor)
    {
        await vendorRepository.AddAsync(vendor);
        return CreatedAtAction(nameof(GetById), new { id = vendor.Id }, vendor);
    }
        
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] Vendor vendor)
    {
        if (id != vendor.Id)
            return BadRequest();
            
        await vendorRepository.UpdateAsync(vendor);
        return NoContent();
    }
        
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await vendorRepository.DeleteAsync(id);
        return NoContent();
    }
}