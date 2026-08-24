using MediatR;
using WebApp.Application.Core;
using static WebApp.Application.Vehiculos.Queries.GetVehiculo.GetVehiculoQuery;
using WebApp.Application.Interfaces;

namespace WebApp.Application.Vehiculos.Queries.GetVehiculo;

public class GetVehiculoQueryHandler : IRequestHandler<GetVehiculoQueryRequest, Result<GetVehiculoResponse>>
{
    private readonly IVehiculoService _vehiculoService;
    public GetVehiculoQueryHandler(IVehiculoService vehiculoService)
    {
        _vehiculoService = vehiculoService;
    }

    public async Task<Result<GetVehiculoResponse>> Handle(GetVehiculoQueryRequest request, CancellationToken cancellationToken)
    {
        var vehiculo = await _vehiculoService.GetVehiculoResponseByIdAsync(request.Id, cancellationToken);

        if (vehiculo is null)
        {
            return Result<GetVehiculoResponse>.Failure("Vehículo no encontrado");
        }

        return Result<GetVehiculoResponse>.Success(vehiculo);
    }
}