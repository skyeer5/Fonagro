using MediatR;
using WebApp.Application.Core;
using WebApp.Application.Interfaces;

namespace WebApp.Application.Vehiculos.Queries.GetVehiculosDetalle;

public class GetVehiculosDetalleQueryHandler : IRequestHandler<GetVehiculosDetalleQuery.GetVehiculosDetalleQueryRequest, Result<PagedList<GetVehiculosDetalleResponse>>>
{
    private readonly IVehiculoService _vehiculoService;
    public GetVehiculosDetalleQueryHandler(IVehiculoService vehiculoService)
    {
        _vehiculoService = vehiculoService;
    }
    public async Task<Result<PagedList<GetVehiculosDetalleResponse>>> Handle(GetVehiculosDetalleQuery.GetVehiculosDetalleQueryRequest request, CancellationToken cancellationToken)
    {
        return await _vehiculoService.GetVehiculosDetalleAsync(request.request);
    }
}