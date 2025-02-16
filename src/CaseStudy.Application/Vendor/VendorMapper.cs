using Riok.Mapperly.Abstractions;

namespace CaseStudy.Application;

[Mapper]
public static partial class VendorMapper
{
    public static partial VendorDto ToDto(Domain.Vendor entity);

    [MapValue(nameof(Domain.Vendor.Id), Use = nameof(GenerateV7Guid))]
    public static partial Domain.Vendor ToEntity(CreateVendorDto dto);

    [MapperIgnoreTarget(nameof(Domain.Vendor.Id))]
    public static partial void Map(UpdateVendorDto dto, Domain.Vendor entity);

    private static Guid GenerateV7Guid() => Guid.CreateVersion7();
}