using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text.Json;

namespace QvaCar.Api.Configuration
{
    internal static class ApiAuthorizationConfiguration
    {
        public static IServiceCollection AddQvaCarApiAuth(this IServiceCollection services, IConfiguration configuration, IWebHostEnvironment env)
        {
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
            var apiOpts = new ApiOptions();
            configuration.GetSection(ApiOptions.SectionName).Bind(apiOpts);

            services.AddAuthorization(authorizationOptions =>
            {
                authorizationOptions.AddPolicy("AnySubscribedUser", policy =>
                {
                    policy.AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme);
                    policy.RequireClaim(QvaCarClaims.SubscriptionLevel);
                });
            });

            if (!env.IsTesting())
            {
                services.AddAuthentication()
                    .AddJwtBearerConfiguration(apiOpts.AuthHostUrl, "qvacar.api.core");
            }
            return services;
        }

        private static AuthenticationBuilder AddJwtBearerConfiguration(this AuthenticationBuilder builder, string issuer, string audience)
        {
            return builder.AddJwtBearer(options =>
            {
                options.Authority = issuer;
                options.Audience = audience;
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ClockSkew = new System.TimeSpan(0, 0, 30)
                };
                options.Events = new JwtBearerEvents()
                {
                    OnChallenge = context =>
                    {
                        context.HandleResponse();
                        context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                        context.Response.ContentType = "application/json";

                        // Ensure we always have an error and error description.
                        if (string.IsNullOrEmpty(context.Error))
                            context.Error = "invalid_token";
                        if (string.IsNullOrEmpty(context.ErrorDescription))
                            context.ErrorDescription = "This request requires a valid JWT access token to be provided";

                        // Add some extra context for expired tokens.
                        if (context.AuthenticateFailure != null && context.AuthenticateFailure.GetType() == typeof(SecurityTokenExpiredException))
                        {
                            var authenticationException = context.AuthenticateFailure as SecurityTokenExpiredException;
                            context.Response.Headers.Add("x-token-expired", authenticationException?.Expires.ToString("o"));
                            context.ErrorDescription = $"The token expired on {authenticationException?.Expires.ToString("o")}";
                        }
                        throw new System.UnauthorizedAccessException("Authentication required!");
                    }
                };
            });
        }


    }




    public static class EnvironmentConfigurationExtensions
    {
        public static bool IsTesting(this IWebHostEnvironment env) => env.EnvironmentName == "Test";
    }

}