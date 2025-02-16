using CaseStudy.Infrastructure.SeedData;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace CaseStudy.Infrastructure;

public static class ApplicationBuilderExtensions
{
   
    public static IApplicationBuilder MapIdentityApi(this IApplicationBuilder applicationBuilder)
    {

        applicationBuilder.UseEndpoints(endpoints =>
        {
            endpoints.MapGroup("/account").MapIdentityApi<IdentityUser>();
        });

        return applicationBuilder;
    }
}