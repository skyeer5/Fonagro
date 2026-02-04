using FluentValidation;
using MediatR;
using WebApp.Application.Core;
using static WebApp.Application.Usuarios.Commands.UsuarioCreate.UsuarioCreateCommand;

namespace WebApp.Application.Usuarios.Commands.UsuarioCreate;

    public class UsuarioCreateCommand
{
    public record UsuarioCreateCommandRequest(UsuarioCreateRequest UsuarioCreateRequest) : IRequest<Result<int>>;
}

public class UsuarioCreateCommandRequestValidator : AbstractValidator<UsuarioCreateCommandRequest>
    {
        public UsuarioCreateCommandRequestValidator()
        {
            RuleFor(x => x.UsuarioCreateRequest).SetValidator(new UsuarioCreateValidator());
        }
    }