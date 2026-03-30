using FluentValidation;
using MediatR;
using WebApp.Application.Core;

namespace WebApp.Application.Behavior;


public sealed class ValidationBehavior<TRequest, TResponse>
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
{
    private readonly IEnumerable<IValidator<TRequest>> _validators;

    public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
    {
        _validators = validators;
    }

    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
         if (_validators.Any())
        {
            var context = new ValidationContext<TRequest>(request);

            var failures = _validators
                .Select(v => v.Validate(context))
                .SelectMany(r => r.Errors)
                .Where(f => f != null)
                .ToList();

            if (failures.Any())
            {
                var error = string.Join(", ", failures.Select(f => f.ErrorMessage));
                
                var type = typeof(TResponse);
                var valueType = type.GetGenericArguments()[0];

                var failureMethod = typeof(Result<>)
                    .MakeGenericType(valueType)
                    .GetMethod(nameof(Result<object>.Failure), new[] { typeof(string) });

                var result = failureMethod!.Invoke(null, new object[] { error });
                }
        }

        return await next();
    }
}
