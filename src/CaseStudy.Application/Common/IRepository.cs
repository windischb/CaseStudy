using CaseStudy.Domain;
using CaseStudy.Domain.Common;

namespace CaseStudy.Application.Common;

public interface IRepository<T> where T : class, IEntity
{
    Task<IEnumerable<T>> GetAllAsync();
    Task<T?> GetByIdAsync(Guid id);
    Task AddAsync(T entity);
    Task UpdateAsync(T entity);
    Task DeleteAsync(Guid id);
}

public interface IVendorRepository : IRepository<Vendor> { }

public interface IBankAccountRepository : IRepository<BankAccount> { }

public interface IContactPersonRepository : IRepository<ContactPerson> { }