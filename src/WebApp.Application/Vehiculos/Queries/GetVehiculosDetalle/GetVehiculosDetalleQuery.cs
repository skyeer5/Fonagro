using MediatR;
using WebApp.Application.Core;

namespace WebApp.Application.Vehiculos.Queries.GetVehiculosDetalle;

public class GetVehiculosDetalleQuery
{
    public record GetVehiculosDetalleQueryRequest(GetVehiculosDetalleRequest request) : IRequest<Result<PagedList<GetVehiculosDetalleResponse>>>;
}