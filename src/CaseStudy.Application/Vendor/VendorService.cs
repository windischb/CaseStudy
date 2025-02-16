using CaseStudy.Application.Common;
using ErrorOr;

namespace CaseStudy.Application;

public class VendorService(IVendorRepository repository): IVendorService
{
    public async Task<IEnumerable<VendorDto>> GetAllAsync()
    {
        var vendors = await repository.GetAllAsync();
        return vendors.Select(VendorMapper.ToDto);
    }

    public async Task<VendorDto?> GetByIdAsync(Guid id)
    {
        var vendor = await repository.GetByIdAsync(id);
        return vendor is null ? null : VendorMapper.ToDto(vendor);
    }

    public async Task<Guid> AddAsync(CreateVendorDto dto)
    {
        var newVendor = VendorMapper.ToEntity(dto);
        await repository.AddAsync(newVendor);
        return newVendor.Id;
    }

    public async Task<ErrorOr<Guid>> UpdateAsync(Guid id, UpdateVendorDto dto)
    {
        var vendor = await repository.GetByIdAsync(id);
        if (vendor is null)
        {
            return Error.NotFound("Vendor.NotFound");
        }

        VendorMapper.Map(dto, vendor);
        await repository.UpdateAsync(vendor);
        return vendor.Id;
    }

    public Task DeleteAsync(Guid id)
    {
        return repository.DeleteAsync(id);
    }
}