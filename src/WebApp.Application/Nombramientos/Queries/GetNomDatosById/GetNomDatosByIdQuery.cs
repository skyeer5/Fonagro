using MediatR;
using WebApp.Application.Core;

namespace WebApp.Application.Nombramientos.Queries.GetNomDatosById;

public class GetNomDatosByIdQuery
{
    public record GetNomDatosByIdQueryRequest(int nombramientoId) : IRequest<Result<GetNomDatosByIdResponse>>;
}