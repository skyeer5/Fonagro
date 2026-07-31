using MediatR;
using WebApp.Application.Core;

namespace WebApp.Application.Puestos.Queries.GetPuestosByUnidad;

public class GetPuestosByUnidadQuery
{
    public record GetPuestosByUnidadQueryRequest(int unidadId) : IRequest<Result<List<GetPuestosByUnidadResponse>>>;
}