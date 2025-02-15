using CaseStudy.Application.Interfaces;
using CaseStudy.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace CaseStudy.Infrastructure;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, string connectionString)
    {
        services.AddDbContext<AppDbContext>(builder => builder.UseNpgsql(connectionString));
        services.AddScoped<IVendorRepository, VendorRepository>();
        services.AddScoped<IBankAccountRepository, BankAccountRepository>();
        services.AddScoped<IContactPersonRepository, ContactPersonRepository>();

        return services;
    }
}