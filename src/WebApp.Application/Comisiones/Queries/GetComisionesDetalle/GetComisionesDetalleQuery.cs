using MediatR;
using WebApp.Application.Core;

namespace WebApp.Application.Comisiones.Queries.GetComisionesDetalle;

public class GetComisionesDetalleQuery
{
    public record GetComisionesDetalleQueryRequest(GetComisionesDetalleRequest request) : IRequest<Result<PagedList<GetComisionesDetalleResponse>>>;
}