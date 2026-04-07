using WebApp.Application.Core;
using WebApp.Application.Vehiculos.Queries.GetVehiculosDetalle;
using WebApp.Domain;

namespace WebApp.Application.Interfaces;

public interface IVehiculoService
{
    Task<bool> VehiculoExistsAsync(int vehiculoId, CancellationToken cancellationToken);
    Task<List<Vehiculo>> GetVehiculosDisponiblesAsync(CancellationToken cancellationToken);
    Task<Vehiculo?> GetVehiculoByIdAsync(int vehiculoId, CancellationToken cancellationToken);
    Task<Result<PagedList<GetVehiculosDetalleResponse>>> GetVehiculosDetalleAsync(GetVehiculosDetalleRequest request);
}