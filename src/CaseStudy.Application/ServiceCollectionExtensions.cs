using CaseStudy.Application.Common;
using Microsoft.Extensions.DependencyInjection;

namespace CaseStudy.Application;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<IVendorService, VendorService>();
        services.AddScoped<IBankAccountService, BankAccountService>();
        services.AddScoped<IContactPersonService, ContactPersonService>();

        return services;
    }
}