using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.OpenApi;
using Microsoft.OpenApi.Models;
using Microsoft.OpenApi.Services;

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
                ["Bearer"] = new OpenApiSecurityScheme
                {
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer", // "bearer" refers to the header name here
                    In = ParameterLocation.Header,
                    BearerFormat = "Json Web Token"
                }
            };
            document.Components ??= new OpenApiComponents();
            document.Components.SecuritySchemes = requirements;

            // Iterate through all paths in the document.
            foreach (var pathPair in document.Paths)
            {
                var pathKey = pathPair.Key.TrimStart('/');
                var pathItem = pathPair.Value;

                // Find all ApiDescriptions that match this path.
                // (This matching logic might need adjustment based on your route templates.)
                var matchingDescriptions = context.DescriptionGroups.SelectMany(g => g.Items)
                    .Where(desc => string.Equals(desc.RelativePath?.TrimEnd('/'), pathKey, StringComparison.OrdinalIgnoreCase))
                    .ToList();

                // For each operation (GET, POST, etc.) in the path:
                foreach (var operationPair in pathItem.Operations)
                {
                    var httpMethod = operationPair.Key.ToString().ToUpperInvariant();
                    var operation = operationPair.Value;

                    // Check if any of the matching ApiDescriptions for this path and method have [Authorize]
                    bool requiresAuth = matchingDescriptions.Any(desc =>
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
}