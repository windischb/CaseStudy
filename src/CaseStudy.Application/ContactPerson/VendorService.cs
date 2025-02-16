using CaseStudy.Application.Common;
using ErrorOr;

namespace CaseStudy.Application;

public class ContactPersonService(IContactPersonRepository repository): IContactPersonService
{
    public async Task<IEnumerable<ContactPersonDto>> GetAllAsync()
    {
        var contactPersons = await repository.GetAllAsync();
        return contactPersons.Select(ContactPersonMapper.ToDto);
    }

    public async Task<ContactPersonDto?> GetByIdAsync(Guid id)
    {
        var contactPerson = await repository.GetByIdAsync(id);
        return contactPerson is null ? null : ContactPersonMapper.ToDto(contactPerson);
    }

    public async Task<Guid> AddAsync(CreateContactPersonDto dto)
    {
        var newContactPerson = ContactPersonMapper.ToEntity(dto);
        await repository.AddAsync(newContactPerson);
        return newContactPerson.Id;
    }

    public async Task<ErrorOr<Guid>> UpdateAsync(Guid id, UpdateContactPersonDto dto)
    {
        var contactPerson = await repository.GetByIdAsync(id);
        if (contactPerson is null)
        {
            return Error.NotFound("Vendor.NotFound");
        }

        ContactPersonMapper.Map(dto, contactPerson);
        await repository.UpdateAsync(contactPerson);
        return contactPerson.Id;
    }

    public Task DeleteAsync(Guid id)
    {
        return repository.DeleteAsync(id);
    }
}