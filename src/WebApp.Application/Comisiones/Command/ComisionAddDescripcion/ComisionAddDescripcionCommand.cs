using MediatR;
using WebApp.Application.Core;

namespace WebApp.Application.Comisiones.Command.ComisionAddDescripcion;

public class ComisionAddDescripcionQuery
{
    public record ComisionAddDescripcionCommandRequest(ComisionAddDescripcionRequest request) : IRequest<Result<int>>;
}