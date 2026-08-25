using MediatR;
using WebApp.Application.Core;
using WebApp.Application.Interfaces;
using static WebApp.Application.Vehiculos.Queries.GetVehiculos.GetVehiculosQuery;

namespace WebApp.Application.Vehiculos.Queries.GetVehiculos;

public class GetVehiculosQueryHandler : IRequestHandler<GetVehiculosQueryRequest, Result<List<GetVehiculosResponse>>>
{
    private readonly IVehiculoService _vehiculoService;

    public GetVehiculosQueryHandler(IVehiculoService vehiculoService)
    {
        _vehiculoService = vehiculoService;
    }

    public async Task<Result<List<GetVehiculosResponse>>> Handle(GetVehiculosQueryRequest request, CancellationToken cancellationToken)
    {
        var vehiculos = await _vehiculoService.GetVehiculosAsync(cancellationToken);
        
        if(vehiculos is null)
        {
            return Result<List<GetVehiculosResponse>>.Failure("Error al obtener los vehiculso");
        }
        return Result<List<GetVehiculosResponse>>.Success(vehiculos);
    }
}