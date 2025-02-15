using CaseStudy.Infrastructure.SeedData;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace CaseStudy.Infrastructure;

public static class ServiceProviderExtensions
{
    public static IServiceProvider AutomaticDatabaseMigration(this IServiceProvider serviceProvider,
        bool enable = false)
    {
        if (enable)
        {
            using var scope = serviceProvider.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            dbContext.Database.Migrate();
            DatabaseSeeder.Initialize(dbContext);
        }
        return serviceProvider;
    }
}