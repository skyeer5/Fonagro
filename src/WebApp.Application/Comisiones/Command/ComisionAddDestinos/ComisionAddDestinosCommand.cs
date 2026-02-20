using MediatR;
using WebApp.Application.Core;

namespace WebApp.Application.Comisiones.Command.ComisionAddDestinos;

public class ComisionAddDestinosCommand
{
    public record ComisionAddDestinosCommandRequest(ComisionAddDestinosRequest request) : IRequest<Result<int>>;
}