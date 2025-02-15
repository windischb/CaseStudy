using CaseStudy.Domain;
using Microsoft.EntityFrameworkCore;

namespace CaseStudy.Infrastructure;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Vendor> Vendors { get; set; }
    public DbSet<BankAccount> BankAccounts { get; set; }
    public DbSet<ContactPerson> ContactPersons { get; set; }
}