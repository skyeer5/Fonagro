using MediatR;
using WebApp.Application.Core;

namespace WebApp.Application.GasolinaPrecios.Command.GasolinaPrecioCreate;

public class GasolinaPrecioCreateCommand
{
    public record GasolinaPrecioCreateCommandRequest(GasolinaPrecioCreateRequest Request) : IRequest<Result<int>>;
}