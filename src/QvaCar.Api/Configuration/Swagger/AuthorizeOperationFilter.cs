using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace QvaCar.Api.Configuration
{

    public partial class ConfigureSwaggerGenOptions
    {
        internal class AuthorizeOperationFilter : IOperationFilter
        {
            public void Apply(OpenApiOperation operation, OperationFilterContext context)
            {
                if (context.MethodInfo.DeclaringType is null)
                    return;

                var allAttributes = context.MethodInfo.DeclaringType.GetCustomAttributes(true)
                                .Union(context.MethodInfo.GetCustomAttributes(true));
                bool requiresAuth = allAttributes.OfType<AuthorizeAttribute>().Any() && !allAttributes.OfType<AllowAnonymousAttribute>().Any();

                if (!requiresAuth)
                    return;

                AddDefaultAuthResponsesToOperation(operation);
                AddAuthorizeRequirementToOperation(operation);
            }

            private static void AddDefaultAuthResponsesToOperation(OpenApiOperation operation)
            {
                operation.Responses.Add(StatusCodes.Status401Unauthorized.ToString(), new OpenApiResponse { Description = nameof(HttpStatusCode.Unauthorized) });
                operation.Responses.Add(StatusCodes.Status403Forbidden.ToString(), new OpenApiResponse { Description = nameof(HttpStatusCode.Forbidden) });
            }

            private static void AddAuthorizeRequirementToOperation(OpenApiOperation operation)
            {
                operation.Security = new List<OpenApiSecurityRequirement>();
                var oauth2SecurityScheme = new OpenApiSecurityScheme()
                {
                    Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "OAuth2" },
                };

                operation.Security.Add(new OpenApiSecurityRequirement() { [oauth2SecurityScheme] = new[] { "OAuth2" } });
            }

        }
    }
}