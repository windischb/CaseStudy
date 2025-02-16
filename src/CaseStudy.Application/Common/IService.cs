using ErrorOr;

namespace CaseStudy.Application.Common;

public interface IService<TDto, TCreateDto, TUpdateDto>
{
    Task<IEnumerable<TDto>> GetAllAsync();
    Task<TDto?> GetByIdAsync(Guid id);
    Task<Guid> AddAsync(TCreateDto dto);
    Task<ErrorOr<Guid>> UpdateAsync(Guid id, TUpdateDto dto);
    Task DeleteAsync(Guid id);
}

public interface IVendorService : IService<VendorDto, CreateVendorDto, UpdateVendorDto> { }
public interface IBankAccountService : IService<BankAccountDto, CreateBankAccountDto, UpdateBankAccountDto> { }
public interface IContactPersonService : IService<ContactPersonDto, CreateContactPersonDto, UpdateContactPersonDto> { }