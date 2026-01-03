using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ordering.Application.Exception
{
    public class UnhandledException<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
    {
        private readonly ILogger<UnhandledException<TRequest, TResponse>> _logger;
        public UnhandledException(ILogger<UnhandledException<TRequest, TResponse>> logger)
        {
            _logger = logger;
        }
        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            try
            {
                return await next();    
            }catch(System.Exception ex)
            {
                var requestName=typeof(TRequest).Name;  
                _logger.LogInformation($"Unhandled Exception for Request {requestName} {ex}");
                throw;
            }
        }
    }
}
