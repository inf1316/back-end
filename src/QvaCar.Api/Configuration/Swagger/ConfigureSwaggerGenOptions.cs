using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;

namespace QvaCar.Api.Configuration
{
    public partial class ConfigureSwaggerGenOptions : IConfigureOptions<SwaggerGenOptions>
    {
        private readonly string _authUrl;

        public ConfigureSwaggerGenOptions(string authUrl)
        {
            _authUrl = authUrl;
        }

        public void Configure(SwaggerGenOptions options)
        {
            options.OperationFilter<AuthorizeOperationFilter>();
            options.AddSecurityDefinition("OAuth2", new OpenApiSecurityScheme
            {
                Type = SecuritySchemeType.OAuth2,
                Flows = new OpenApiOAuthFlows
                {
                    AuthorizationCode = new OpenApiOAuthFlow
                    {
                        AuthorizationUrl = new Uri($"{_authUrl}/connect/authorize"),
                        TokenUrl = new Uri($"{_authUrl}/connect/token"),
                        Scopes = new Dictionary<string, string>
                        {
                            { "openid profile qvacar.api.core province subscription_level" , "Qva Car Api" }
                        },
                    }
                },
                Description = "Qva Car Open Id Scheme"
            });
        }
    }
}