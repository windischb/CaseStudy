using Riok.Mapperly.Abstractions;

namespace CaseStudy.Application;

[Mapper]
public static partial class BankAccountMapper
{
    public static partial BankAccountDto ToDto(Domain.BankAccount entity);

    [MapValue(nameof(Domain.BankAccount.Id), Use = nameof(GenerateV7Guid))]
    public static partial Domain.BankAccount ToEntity(CreateBankAccountDto dto);

    [MapperIgnoreTarget(nameof(Domain.BankAccount.Id))]
    public static partial void Map(UpdateBankAccountDto dto, Domain.BankAccount entity);

    private static Guid GenerateV7Guid() => Guid.CreateVersion7();

}