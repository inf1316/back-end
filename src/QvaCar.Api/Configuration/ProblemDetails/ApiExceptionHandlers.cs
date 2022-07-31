using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QvaCar.Application.Exceptions;
using QvaCar.Seedwork.Domain;
using System;

namespace QvaCar.Api.Configuration
{
    internal static class ApiExceptionHandlers
    {
        private static string UnhandledExceptionTitle => "Whoops. Something went wrong";
        private static string DomainExceptionTitle => "Whoops looks like there is a problem with your request";
        private static string DomainIsInInvalidStateExceptionTitle => "Whoops looks like there is a problem with your account get in touch with us to fix it";
        private static string ValidationExceptionTitle => "One or more validation failures have occurred.";
        private static string EntityNotFoundExceptionTitle => "Entity not found.";
        private static string DomainInvalidOperationExceptionTitle => "Invalid Operation.";
        private static string UnauthorizedAccessExceptionTitle => "You need to authenticate.";
        private static string ForbiddenAccessExceptionTitle => "You dont have access to this.";

        public static ProblemDetails FluentValidationExceptionHandler(ValidationException ex)
        {
            return new ValidationProblemDetails(ex.Errors)
            {
                Detail = ex.Message,
                Status = StatusCodes.Status400BadRequest,
                Title = ValidationExceptionTitle,
            };
        }

        public static ProblemDetails UnauthorizedAccessExceptionHandler(UnauthorizedAccessException ex)
        {
            return new ValidationProblemDetails()
            {
                Detail = ex.Message,
                Status = StatusCodes.Status401Unauthorized,
                Title = UnauthorizedAccessExceptionTitle,
            };
        }

        public static ProblemDetails ForbiddenAccessExceptionHandler(ForbiddenAccessException ex)
        {
            return new ValidationProblemDetails()
            {
                Detail = ex.Message,
                Status = StatusCodes.Status403Forbidden,
                Title = ForbiddenAccessExceptionTitle,
            };
        }

        public static ProblemDetails DomainIsInInvalidStateExceptionHandler(DomainIsInInvalidStateException ex)
        {
            return new ProblemDetails
            {
                Detail = ex.Message,
                Status = StatusCodes.Status500InternalServerError,
                Title = DomainIsInInvalidStateExceptionTitle
            };
        }

        public static ProblemDetails DomainValidationExceptionHandler(DomainValidationException ex)
        {
            return new ValidationProblemDetails(ex.Errors)
            {
                Detail = ex.Message,
                Status = StatusCodes.Status400BadRequest,
                Title = ValidationExceptionTitle,
            };
        }

        public static ProblemDetails NotFoundExceptionHandler(EntityNotFoundException ex)
        {
            return new ValidationProblemDetails()
            {
                Detail = ex.Message,
                Status = StatusCodes.Status404NotFound,
                Title = EntityNotFoundExceptionTitle,
            };
        }

        public static ProblemDetails DomainInvalidOperationExceptionHandler(DomainInvalidOperationException ex)
        {
            return new ValidationProblemDetails()
            {
                Detail = ex.Message,
                Status = StatusCodes.Status400BadRequest,
                Title = DomainInvalidOperationExceptionTitle,
            };
        }

        public static ProblemDetails DomainExceptionHandler(DomainException ex)
        {
            return new ProblemDetails
            {
                Detail = ex.Message,
                Status = StatusCodes.Status400BadRequest,
                Title = DomainExceptionTitle
            };
        }

        public static ProblemDetails UnhandledExceptionHandler(Exception ex)
        {
            return new ProblemDetails
            {
                Detail = ex.Message,
                Status = StatusCodes.Status500InternalServerError,
                Title = UnhandledExceptionTitle
            };
        }
    }
}