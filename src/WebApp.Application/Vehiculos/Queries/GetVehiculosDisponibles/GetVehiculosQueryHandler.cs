using MediatR;
using Microsoft.IdentityModel.Tokens;
using WebApp.Application.Core;
using WebApp.Application.Interfaces;
using static WebApp.Application.Vehiculos.Queries.GetVehiculosDisponibles.GetVehiculosDisponiblesQuery;

namespace WebApp.Application.Vehiculos.Queries.GetVehiculosDisponibles;

public class GetVehiculosDisponiblesQueryHandler : IRequestHandler<GetVehiculosDisponiblesQueryRequest, Result<List<GetVehiculosDisponiblesResponse>>>
{
    private readonly IVehiculoService _vehiculoService;
    public GetVehiculosDisponiblesQueryHandler(IVehiculoService vehiculoService)
    {
        _vehiculoService = vehiculoService;
    }

    public async Task<Result<List<GetVehiculosDisponiblesResponse>>> Handle(GetVehiculosDisponiblesQueryRequest request, CancellationToken cancellationToken)
    {
        var vehiculos = await _vehiculoService.GetVehiculosDisponiblesAsync(cancellationToken);
        if(vehiculos.IsNullOrEmpty())
        {
            return Result<List<GetVehiculosDisponiblesResponse>>.Failure("No hay vehículos disponibles");
        }
        var response = vehiculos.Select(v => new GetVehiculosDisponiblesResponse
        {
            id = v.VehiculoId,
            Descripcion = $"{v.Marca} {v.Modelo} - {v.Placa}"
        }).ToList();
        return Result<List<GetVehiculosDisponiblesResponse>>.Success(response);
    }
}
