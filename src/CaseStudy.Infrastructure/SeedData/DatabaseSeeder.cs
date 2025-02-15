using System.Text.Json;
using CaseStudy.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CaseStudy.Infrastructure.SeedData;

public static class DatabaseSeeder
{
    public static void Initialize(AppDbContext context)
    {
        SeedData(context, "vendors.json", context.Vendors);
        SeedData(context, "bankaccounts.json", context.BankAccounts);
        SeedData(context, "contactpersons.json", context.ContactPersons);
    }

    private static void SeedData<T>(AppDbContext context, string fileName, DbSet<T> dbSet) where T : class, IEntity
    {

        var resourceName = $"CaseStudy.Infrastructure.SeedData.{fileName}";
        using var stream = typeof(DatabaseSeeder).Assembly.GetManifestResourceStream(resourceName);
        if (stream == null)
        {
            Console.WriteLine($"Embedded resource not found: {resourceName}");
            return;
        }

        using var reader = new StreamReader(stream);
        var jsonData = reader.ReadToEnd();

        var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        var items = JsonSerializer.Deserialize<List<T>>(jsonData, options);

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
            
        context.SaveChanges();
    }
}