
using CaseStudy.Api.Helpers;
using CaseStudy.Application;
using CaseStudy.Infrastructure;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.OpenApi.Models;

namespace CaseStudy.Api;


public class Startup(IConfiguration configuration)
{
    public StartUpConfiguration Configuration { get; } = configuration.Get<StartUpConfiguration>()!;
    
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddSingleton(_ => Configuration);

        services
            .AddStackExchangeRedisOutputCache(opts =>
            {
                opts.Configuration = Configuration.RedisSettings.ConnectionString;
                opts.InstanceName = "CaseStudy";
                
            })
            .AddOutputCache(opts =>
            {
                opts.DefaultExpirationTimeSpan = TimeSpan.FromMinutes(10);
                opts.AddPolicy("TagById", new TagFromRouteValuesOutputCachePolicy(request =>
                {
                    var actionDescriptor = request.HttpContext.GetEndpoint()?.Metadata
                        .GetMetadata<ControllerActionDescriptor>();
                    var s = request.HttpContext.Request.RouteValues["id"]?.ToString();
                    if (actionDescriptor is null)
                    {
                        return null;
                    }
                    if (string.IsNullOrWhiteSpace(s))
                    {
                        return $"{actionDescriptor.ControllerName}-All";
                    }
                    return $"TagById-{actionDescriptor.ControllerName}-{s}";
                }));
            });

        
        services.AddAuthorization(options =>
            {
                options.AddPolicy("Admin", builder => builder.RequireRole("Administrator"));
            })
            .AddAuthentication();

        services.AddApplicationServices();
        services.AddInfrastructureServices(Configuration.DbSettings.ConnectionString);

        services.AddOpenApi(options =>
        {
            options.AddDocumentTransformer((document, _, _) =>
            {
                var newServers = document.Servers.Select(s =>
                    new OpenApiServer { Url = IPAddressHelper.GenerateOpenApiUri(s.Url).TrimEnd('/') }
                ).ToList();

                document.Servers.Clear();
                document.Servers = newServers;
                return Task.CompletedTask;
            });

            options.AddDocumentTransformer<BearerSecuritySchemeTransformer>();
        });

        services.AddControllers();

        services.AddResponseCompression();

    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {

        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }
        app.UseResponseCompression();

        app.UseRouting();

        app.UseAuthentication();
        app.UseAuthorization();
        app.UseOutputCache();

        app.MapIdentityApi();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapOpenApi();
            endpoints.MapControllers();
        });

        app.UseSwaggerUI(opts =>
        {
            opts.SwaggerEndpoint("/openapi/v1.json", "CaseStudy Api");

        });

    }


}

