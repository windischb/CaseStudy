using CaseStudy.Application;
using CaseStudy.Application.Common;
using ErrorOr;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;

namespace CaseStudy.Api.Controllers;

[Route("api/vendors")]
[ApiController]
[Authorize]
public class VendorsController(IVendorService vendorService, IOutputCacheStore cacheStore) : ControllerBase
{
    [HttpGet]
    [OutputCache(PolicyName = "TagById")]
    public async Task<ActionResult<IEnumerable<VendorDto>>> GetAll()
    {
        var vendors = await vendorService.GetAllAsync();
        return Ok(vendors);
    }
        
    [HttpGet("{id}")]
    [OutputCache(PolicyName = "TagById")]
    public async Task<ActionResult<VendorDto>> GetById(Guid id)
    {
        var vendor = await vendorService.GetByIdAsync(id);
        if (vendor == null)
            return NotFound();
        return Ok(vendor);
    }
        
    [HttpPost]
    [Authorize(Policy = "Admin")]
    public async Task<IActionResult> Create([FromBody] CreateVendorDto dto)
    {
        var newVendorId = await vendorService.AddAsync(dto);
        await cacheStore.EvictByTagAsync("Vendors-All", HttpContext.RequestAborted);
        return CreatedAtAction(nameof(GetById), new { id = newVendorId }, dto);
    }
        
    [HttpPut("{id}")]
    [Authorize(Policy = "Admin")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateVendorDto dto)
    {
        var updateResult = await vendorService.UpdateAsync(id, dto);
        if (updateResult.IsError)
        {
            if (updateResult.FirstError == Error.NotFound())
            {
                return NotFound(updateResult.FirstError.Code);
            }
            return BadRequest(updateResult.FirstError);
        }
        await cacheStore.EvictByTagAsync("Vendors-All", HttpContext.RequestAborted);
        await cacheStore.EvictByTagAsync($"TagById-Vendors-{id}", HttpContext.RequestAborted);
        return NoContent();
    }
        
    [HttpDelete("{id}")]
    [Authorize(Policy = "Admin")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await vendorService.DeleteAsync(id);
        await cacheStore.EvictByTagAsync("Vendors-All", HttpContext.RequestAborted);
        await cacheStore.EvictByTagAsync($"TagById-Vendors-{id}", HttpContext.RequestAborted);
        return NoContent();
    }
}