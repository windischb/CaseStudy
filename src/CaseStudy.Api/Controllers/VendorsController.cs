using CaseStudy.Application.Interfaces;
using CaseStudy.Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;

namespace CaseStudy.Api.Controllers;

[Route("api/vendors")]
[ApiController]
public class VendorsController(IVendorRepository vendorRepository, IOutputCacheStore cacheStore) : ControllerBase
{
    [HttpGet]
    [OutputCache(Tags = ["Vendors-All"])]
    public async Task<ActionResult<IEnumerable<Vendor>>> GetAll()
    {
        var vendors = await vendorRepository.GetAllAsync();
        return Ok(vendors);
    }
        
    [HttpGet("{id}")]
    [OutputCache(PolicyName = "TagById")]
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
        await cacheStore.EvictByTagAsync("Vendors-All", this.HttpContext.RequestAborted);
        return CreatedAtAction(nameof(GetById), new { id = vendor.Id }, vendor);
    }
        
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] Vendor vendor)
    {
        if (id != vendor.Id)
            return BadRequest();
            
        await vendorRepository.UpdateAsync(vendor);
        await cacheStore.EvictByTagAsync("Vendors-All", this.HttpContext.RequestAborted);
        await cacheStore.EvictByTagAsync($"TagById-Vendors-{id}", this.HttpContext.RequestAborted);
        return NoContent();
    }
        
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await vendorRepository.DeleteAsync(id);
        await cacheStore.EvictByTagAsync("Vendors-All", this.HttpContext.RequestAborted);
        await cacheStore.EvictByTagAsync($"TagById-Vendors-{id}", this.HttpContext.RequestAborted);
        return NoContent();
    }
}