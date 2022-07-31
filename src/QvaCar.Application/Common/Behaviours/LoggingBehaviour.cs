using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace QvaCar.Application.Common.Behaviours
{
    public class LoggingBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
    {
        private readonly ILogger<LoggingBehaviour<TRequest, TResponse>> _logger;

        public LoggingBehaviour(ILogger<LoggingBehaviour<TRequest, TResponse>> logger)
        {
            _logger = logger;
        }

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            // Request
            _logger.LogInformation($"Handling {typeof(TRequest).Name}");

            Type myType = request.GetType();
            IList<PropertyInfo> props = new List<PropertyInfo>(myType.GetProperties());

            foreach (var prop in props)
            {
                object? propValue = prop?.GetValue(request, null);
                _logger.LogInformation("{Property} : {@Value}", prop?.Name, propValue);
            }

            // Response
            try
            {
                var response = await next();
                _logger.LogInformation($"Handled { typeof(TResponse).Name }");
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"Error Message {ex.Message }, Trace Error { ex.StackTrace }");
                throw;
            }
        }
    }
}
