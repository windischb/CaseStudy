using CaseStudy.Infrastructure.SeedData;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace CaseStudy.Infrastructure;

public static class DbMigrator
{
    public static async Task Migrate(IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var appDbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        await appDbContext.Database.MigrateAsync();
        var authDbContext = scope.ServiceProvider.GetRequiredService<AuthDbContext>();
        await authDbContext.Database.MigrateAsync();

        await AppDataSeeder.SeedAsync(appDbContext);
        await AuthDataSeeder.SeedAsync(scope.ServiceProvider);
    }
}