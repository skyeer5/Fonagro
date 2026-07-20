using WebApp.Application.Core;
using WebApp.Domain.Vehiculos;

namespace WebApp.Application.Interfaces;

public interface IVehiculoRepository
{
    Task<Result<int>> CreateVehiculoAsync(Vehiculo vehiculo, CancellationToken cancellationToken);
}