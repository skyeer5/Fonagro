namespace WebApp.Application.Interfaces;

public interface IGasolinaPrecioService
{
    Task<decimal?> GetPrecioActualByVehiculoIdAsync(int vehiculoId, CancellationToken cancellationToken);
}