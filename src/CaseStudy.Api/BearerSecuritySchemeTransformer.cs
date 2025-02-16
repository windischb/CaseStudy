using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.OpenApi;
using Microsoft.OpenApi.Models;

namespace CaseStudy.Api;

internal sealed class BearerSecuritySchemeTransformer(IAuthenticationSchemeProvider authenticationSchemeProvider) : IOpenApiDocumentTransformer
{
    public async Task TransformAsync(OpenApiDocument document, OpenApiDocumentTransformerContext context, CancellationToken cancellationToken)
    {
        var authenticationSchemes = await authenticationSchemeProvider.GetAllSchemesAsync();
        if (authenticationSchemes.Any(authScheme => authScheme.Name == "Identity.Bearer"))
        {
            var requirements = new Dictionary<string, OpenApiSecurityScheme>
            {
                ["Bearer"] = new()
                {
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer",
                    In = ParameterLocation.Header,
                    BearerFormat = "Json Web Token"
                }
            };
            document.Components ??= new OpenApiComponents();
            document.Components.SecuritySchemes = requirements;

            MarkAuthorizedEndpoints(document, context);
        }
    }

    public void MarkAuthorizedEndpoints(OpenApiDocument document, OpenApiDocumentTransformerContext context)
    {

        foreach (var (key, value) in document.Paths)
        {
            var pathKey = key.TrimStart('/');

            var matchingDescriptions = context.DescriptionGroups.SelectMany(g => g.Items)
                .Where(desc => string.Equals(desc.RelativePath?.TrimEnd('/'), pathKey, StringComparison.OrdinalIgnoreCase))
                .ToList();

            foreach (var operationPair in value.Operations)
            {
                var httpMethod = operationPair.Key.ToString().ToUpperInvariant();
                var operation = operationPair.Value;

                var requiresAuth = matchingDescriptions.Any(desc =>
                    string.Equals(desc.HttpMethod, httpMethod, StringComparison.OrdinalIgnoreCase)
                    && desc.ActionDescriptor.EndpointMetadata.Any(m => m is AuthorizeAttribute)
                );

                if (requiresAuth)
                {
                    operation.Security.Add(new OpenApiSecurityRequirement
                    {
                        [new OpenApiSecurityScheme { Reference = new OpenApiReference { Id = "Bearer", Type = ReferenceType.SecurityScheme } }] = Array.Empty<string>()
                    });
                }
            }
        }
    }
}