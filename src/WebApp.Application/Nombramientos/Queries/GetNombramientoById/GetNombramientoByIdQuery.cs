using MediatR;
using WebApp.Application.Core;

namespace WebApp.Application.Nombramientos.Queries.GetNombramientoById;

public class GetNombramientoByIdQuery
{
    public record GetNombramientoByIdQueryRequest(int NombramientoId) : IRequest<Result<GetNombramientoByIdResponse>>;
}