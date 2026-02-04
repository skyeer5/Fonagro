using WebApp.Application.Core;
using WebApp.Domain;

namespace WebApp.Application.Interfaces;

public interface IVehiculoService
{
    Task<bool> VehiculoExistsAsync(int vehiculoId, CancellationToken cancellationToken);
    Task<List<Vehiculo>> GetVehiculosDisponiblesAsync(CancellationToken cancellationToken);
    Task<Vehiculo?> GetVehiculoByIdAsync(int vehiculoId, CancellationToken cancellationToken);
}