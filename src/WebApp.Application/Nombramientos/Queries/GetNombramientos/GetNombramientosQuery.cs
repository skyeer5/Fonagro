using MediatR;
using WebApp.Application.Core;

namespace WebApp.Application.Nombramientos.Queries.GetNombramientos;

public class GetNomParaAprobarQuery
{
    public record GetNombramientosQueryRequest(GetNombramientosRequest request) : IRequest<Result<PagedList<GetNombramientosResponse>>>;
}