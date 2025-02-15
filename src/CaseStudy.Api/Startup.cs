
using CaseStudy.Api.Helpers;
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
                    if (actionDescriptor is null || string.IsNullOrWhiteSpace(s))
                    {
                        return null;
                    }
                    return $"TagById-{actionDescriptor.ControllerName}-{s}";
                }));
            });

        services.AddInfrastructureServices(Configuration.DbSettings.ConnectionString);

        services.AddOpenApi(options =>
        {
            options.AddDocumentTransformer((document, _, _) =>
            {
                var newServers = document.Servers.Select(s => 
                    new OpenApiServer { Url = IPAddressHelper.GenerateOpenApiUri(s.Url).TrimEnd('/')}
                ).ToList();
                
                document.Servers.Clear();
                document.Servers = newServers;
                return Task.CompletedTask;
            });
           
        });

        services.AddControllers();

        services.AddResponseCompression();

    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        app.ApplicationServices.AutomaticDatabaseMigration(true);

        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }
        app.UseResponseCompression();

        app.UseRouting();
        app.UseOutputCache();

        if (env.IsDevelopment())
        {
            app.UseEndpoints((e) =>
            {
                e.MapOpenApi();
            });
            app.UseSwaggerUI(opts =>
            {
                opts.SwaggerEndpoint("/openapi/v1.json", "CaseStudy Api");
               
            });
        }
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });

    }


}

