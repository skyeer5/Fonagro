using FluentValidation;
using MediatR;
using WebApp.Application.Comisiones.ComisionCreate;
using WebApp.Application.Core;
using static WebApp.Application.Comision.ComisionCreate.ComisionCreateCommand;

namespace WebApp.Application.Comision.ComisionCreate;

public class ComisionCreateCommand
{
    public record ComisionCreateCommandRequest(ComisionCreateRequest ComisionCreateRequest) : IRequest<Result<int>>;
}

public class ComisionCreateCommandRequestValidator : AbstractValidator<ComisionCreateCommandRequest>
{
    public ComisionCreateCommandRequestValidator()
    {
        RuleFor(x => x.ComisionCreateRequest).SetValidator(new ComisionCreateValidator());
    }
}