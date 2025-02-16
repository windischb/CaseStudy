using Riok.Mapperly.Abstractions;

namespace CaseStudy.Application;

[Mapper]
public static partial class ContactPersonMapper
{
    public static partial ContactPersonDto ToDto(Domain.ContactPerson entity);

    [MapValue(nameof(Domain.ContactPerson.Id), Use = nameof(GenerateV7Guid))]
    public static partial Domain.ContactPerson ToEntity(CreateContactPersonDto dto);

    [MapperIgnoreTarget(nameof(Domain.ContactPerson.Id))]
    public static partial void Map(UpdateContactPersonDto dto, Domain.ContactPerson entity);

    private static Guid GenerateV7Guid() => Guid.CreateVersion7();

}