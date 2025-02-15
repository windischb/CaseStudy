
using CaseStudy.Api.Helpers;
using CaseStudy.Infrastructure;
using Microsoft.OpenApi.Models;

namespace CaseStudy.Api;


public class Startup(IConfiguration configuration)
{
    public StartUpConfiguration Configuration { get; } = configuration.Get<StartUpConfiguration>()!;
    
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddSingleton(_ => Configuration);

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

