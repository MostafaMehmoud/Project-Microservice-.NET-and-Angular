using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using MediatR;
using Ordering.Application.Exception;

namespace Ordering.Application.Behaviors
{
    public class Validationbehavior<TRequest,TResponse>:IPipelineBehavior<TRequest,TResponse> where TRequest:IRequest<TResponse>
    {
        private readonly IEnumerable<IValidator<TRequest>> _validators;
        public Validationbehavior(IEnumerable<IValidator<TRequest>> validators)
        {
            _validators = validators;
        }
        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            if (_validators.Any())
            {
                var context=new ValidationContext<TRequest>(request);
                var validationResult=await Task.WhenAll(_validators
                    .Select(o=>o.ValidateAsync(context, cancellationToken)));
                var failures = validationResult.SelectMany(o => o.Errors)
                    .Where(o => o != null)
                    .ToList();
                if (failures.Count != 0)
                    throw new ValidatorException(failures);

            }
            return await next();
        }
    }
}
