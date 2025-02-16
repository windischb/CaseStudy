using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace CaseStudy.Infrastructure.SeedData;

public static class AuthDataSeeder
{

    public static async Task SeedAsync(IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        
        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();
        
        var adminRole = "Administrator";
        if (!await roleManager.RoleExistsAsync(adminRole))
        {
            await roleManager.CreateAsync(new IdentityRole(adminRole));
        }
        
        var adminEmail = "admin@domain.com";
        var adminUser = await userManager.FindByEmailAsync(adminEmail);
        if (adminUser == null)
        {
            adminUser = new IdentityUser
            {
                UserName = adminEmail,
                Email = adminEmail,
                EmailConfirmed = true
            };
            var createAdminResult = await userManager.CreateAsync(adminUser, "ABC12abc!");
            if (createAdminResult.Succeeded)
            {
                await userManager.AddToRoleAsync(adminUser, adminRole);
            }
            else
            {
                throw new Exception($"Failed to create admin user: {string.Join(", ", createAdminResult.Errors)}");
            }
        }
        
        var userEmail = "user1@domain.com";
        var normalUser = await userManager.FindByEmailAsync(userEmail);
        if (normalUser == null)
        {
            normalUser = new IdentityUser
            {
                UserName = userEmail,
                Email = userEmail,
                EmailConfirmed = true
            };
            var createUserResult = await userManager.CreateAsync(normalUser, "ABC12abc!");
            if (!createUserResult.Succeeded)
            {
                throw new Exception($"Failed to create normal user: {string.Join(", ", createUserResult.Errors)}");
            }
        }
    }
}