using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.IO;
using System.Reflection;

namespace QvaCar.Api.Configuration
{


    internal static class SwaggerConfiguration
    {

        public static IServiceCollection ConfigureCustomSwagger(this IServiceCollection services, IConfiguration configuration, IWebHostEnvironment env)
        {
            if (env.IsTesting())
                return services;

            var apiOpts = new ApiOptions();
            configuration.GetSection(ApiOptions.SectionName).Bind(apiOpts);

            services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerGenOptions>(services => new ConfigureSwaggerGenOptions(apiOpts.AuthHostUrl));
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "V1",
                    Title = "QvaCar Project",
                    Description = "Backend Api For QvaCar",
                });
                c.DescribeAllParametersInCamelCase();
                c.CustomSchemaIds(t => t.Name);
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });

            return services;
        }

        public static IApplicationBuilder UseCustomSwagger(this IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsTesting())
                return app;

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.OAuthClientId("qvacar.test.swagger");
                c.OAuthClientSecret(string.Empty);
                c.OAuthAppName("QvaCar API");
                c.OAuthScopeSeparator(" ");
                c.OAuthUsePkce();
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "QvaCar API V1");
            });
            return app;
        }
    }
}