using Hellang.Middleware.ProblemDetails;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using QvaCar.Application.Exceptions;
using QvaCar.Seedwork.Domain;
using System;

namespace QvaCar.Api.Configuration
{
    internal static class ProblemDetailsConfiguration
    {
        private static string ValidationErrorMessage => "Please refer to the errors property for additional details.";
        private static string ErrorJsonContentType => "application/problem+json";
        private static string ErrorXmlContentType => "application/problem+xml";


        public static IServiceCollection ConfigureProblemDetails(this IServiceCollection services)
        {
            services.AddProblemDetails(opts =>
            {
                opts.IncludeExceptionDetails = (_, __) => false;
                opts.MapApplicationExceptions();
                opts.MapDomainExceptions();
                opts.Map<Exception>(ex => ApiExceptionHandlers.UnhandledExceptionHandler(ex));
                opts.IsProblem = ShouldUseProblemDetails;
            });
            return services;
        }

        private static void MapApplicationExceptions(this ProblemDetailsOptions opts)
        {
            opts.Map<ValidationException>(ex => ApiExceptionHandlers.FluentValidationExceptionHandler(ex));
            opts.Map<UnauthorizedAccessException>(ex => ApiExceptionHandlers.UnauthorizedAccessExceptionHandler(ex));
            opts.Map<ForbiddenAccessException>(ex => ApiExceptionHandlers.ForbiddenAccessExceptionHandler(ex));
        }

        private static void MapDomainExceptions(this ProblemDetailsOptions opts)
        {
            opts.Map<DomainIsInInvalidStateException>(ex => ApiExceptionHandlers.DomainIsInInvalidStateExceptionHandler(ex));
            opts.Map<DomainValidationException>(ex => ApiExceptionHandlers.DomainValidationExceptionHandler(ex));
            opts.Map<EntityNotFoundException>(ex => ApiExceptionHandlers.NotFoundExceptionHandler(ex));
            opts.Map<DomainInvalidOperationException>(ex => ApiExceptionHandlers.DomainInvalidOperationExceptionHandler(ex));
          
            opts.Map<DomainException>(ex => ApiExceptionHandlers.DomainExceptionHandler(ex));
        }

        public static IApplicationBuilder UseCustomProblemDetails(this IApplicationBuilder app)
        {
            app.UseProblemDetails();
            return app;
        }


        public static IActionResult ProblemDetailsApiBehaviorConfiguration(ActionContext context)
        {
            var problemDetails = new ValidationProblemDetails(context.ModelState)
            {
                Instance = context.HttpContext.Request.Path,
                Status = StatusCodes.Status400BadRequest,
                Type = $"https://httpstatuses.com/400",
                Detail = ValidationErrorMessage
            };
            return new BadRequestObjectResult(problemDetails)
            {
                ContentTypes = {
                    ErrorJsonContentType,
                    ErrorXmlContentType
                    }
            };
        }


        private static bool ShouldUseProblemDetails(HttpContext context)
        {
            string path = context.Request.Path;
            if (!path.StartsWith("/api"))
                return false;

            if (!IsProblemStatusCode(context.Response.StatusCode))
                return false;

            if (context.Response.ContentLength.HasValue)
                return false;

            if (string.IsNullOrEmpty(context.Response.ContentType))
                return true;

            return false;
        }
        private static bool IsProblemStatusCode(int? statusCode)
        {
            return statusCode switch
            {
                >= 600 => false,
                < 400 => false,
                null => false,
                _ => true
            };
        }
    }

}