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
                var error = string.Join(
                    Environment.NewLine,
                    failures.Select(f => f.ErrorMessage)
                );

                var responseType = typeof(TResponse);

                if (responseType.IsGenericType &&
                    responseType.GetGenericTypeDefinition() == typeof(Result<>))
                {
                    var valueType = responseType.GetGenericArguments()[0];

                    var resultType = typeof(Result<>)
                        .MakeGenericType(valueType);

                    var failureMethod = resultType.GetMethod(
                        "Failure",
                        new[] { typeof(string) });

                    var failureResult = failureMethod!.Invoke(
                        null,
                        new object[] { error });

                    return (TResponse)failureResult!;
                }

                throw new InvalidOperationException(
                    $"El tipo {responseType.Name} no es compatible con Result<T>");
            }
        }

        return await next();
    }
}
