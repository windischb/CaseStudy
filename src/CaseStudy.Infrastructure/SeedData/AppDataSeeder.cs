using System.Text.Json;
using CaseStudy.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CaseStudy.Infrastructure.SeedData;

public static class AppDataSeeder
{

    private static readonly JsonSerializerOptions _jsonSerializerOptions = new() { PropertyNameCaseInsensitive = true };
    public static async Task SeedAsync(AppDbContext context)
    {
        await SeedDataAsync(context, "vendors.json", context.Vendors);
        await SeedDataAsync(context, "bankaccounts.json", context.BankAccounts);
        await SeedDataAsync(context, "contactpersons.json", context.ContactPersons);
    }

    private static async Task SeedDataAsync<T>(AppDbContext context, string fileName, DbSet<T> dbSet) where T : class, IEntity
    {

        var resourceName = $"CaseStudy.Infrastructure.SeedData.{fileName}";
        await using var stream = typeof(AppDataSeeder).Assembly.GetManifestResourceStream(resourceName);
        if (stream == null)
        {
            Console.WriteLine($"Embedded resource not found: {resourceName}");
            return;
        }

        var items = await JsonSerializer.DeserializeAsync<List<T>>(stream, _jsonSerializerOptions);

        if (items?.Any() != true)
            return;

        foreach (var item in items)
        {
            var existingEntity = context.Find<T>(item.Id);
            if (existingEntity is null)
            {
                dbSet.Add(item);
            }
                
        }
            
        await context.SaveChangesAsync();
    }
}