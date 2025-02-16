using CaseStudy.Application.Common;
using CaseStudy.Domain;
using CaseStudy.Domain.Common;
using Microsoft.EntityFrameworkCore;

namespace CaseStudy.Infrastructure.Repositories;

public class GenericRepository<T>(AppDbContext dbContext) : IRepository<T>
    where T : class, IEntity
{
    public async Task<IEnumerable<T>> GetAllAsync()
    {
        return await dbContext.Set<T>().ToListAsync();
    }

    public async Task<T?> GetByIdAsync(Guid id)
    {
        return await dbContext.Set<T>().FindAsync(id);
    }

    public async Task AddAsync(T entity)
    {
        dbContext.Add(entity);
        await dbContext.SaveChangesAsync();
    }

    public async Task UpdateAsync(T entity)
    {
        dbContext.Update(entity);
        await dbContext.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var entity = await GetByIdAsync(id);
        if (entity is not null)
        {
            dbContext.Remove(entity);
            await dbContext.SaveChangesAsync();
        }
            
    }
}

public class VendorRepository(AppDbContext dbContext) 
    : GenericRepository<Vendor>(dbContext), IVendorRepository;

public class BankAccountRepository(AppDbContext dbContext)
    : GenericRepository<BankAccount>(dbContext), IBankAccountRepository;

public class ContactPersonRepository(AppDbContext dbContext)
    : GenericRepository<ContactPerson>(dbContext), IContactPersonRepository;