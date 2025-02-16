using CaseStudy.Application.Common;
using CaseStudy.Infrastructure.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace CaseStudy.Infrastructure;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, string connectionString)
    {
        services.AddDbContext<AppDbContext>(builder => builder.UseNpgsql(connectionString));
        services.AddDbContext<AuthDbContext>(builder => builder.UseNpgsql(connectionString));

        services.AddIdentityApiEndpoints<IdentityUser>()
            .AddRoles<IdentityRole>()
            .AddEntityFrameworkStores<AuthDbContext>();

        services.AddScoped<IVendorRepository, VendorRepository>();
        services.AddScoped<IBankAccountRepository, BankAccountRepository>();
        services.AddScoped<IContactPersonRepository, ContactPersonRepository>();

        return services;
    }
}