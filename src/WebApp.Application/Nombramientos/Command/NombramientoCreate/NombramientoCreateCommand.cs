using FluentValidation;
using MediatR;
using WebApp.Application.Core;
using static WebApp.Application.Nombramientos.Command.NombramientoCreate.NombramientoCreateCommand;

namespace WebApp.Application.Nombramientos.Command.NombramientoCreate;

public class NombramientoCreateCommand
{
    public record NombramientoCreateCommandRequest(NombramientoCreateRequest request) : IRequest<Result<int>>;
    
}
public class NombramientoCreateCommandRequestValidator : AbstractValidator<NombramientoCreateCommandRequest>
{
    public NombramientoCreateCommandRequestValidator()
    {
        RuleFor(x => x.request).SetValidator(new NombramientoCreateValidator());
    }
}