using WebApp.Application.Gasolinas.Queries.GetGasolinas;
using WebApp.Application.Gasolinas.Queries.GetGasolinasWithPrecio;

namespace WebApp.Application.Interfaces;

public interface IGasolinaService
{
    Task<bool> GasolinaExistsAsync(int gasolinaId, CancellationToken cancellationToken);
    Task<string?> GetNombreByIdAsync(int gasolinaId, CancellationToken cancellationToken);
    Task<List<GetGasolinasResponse>?> GetGasolinasListAsync(CancellationToken cancellationToken);
    Task<List<GetGasolinasWithPrecioResponse>?> GetGasolinasWithPrecioListAsync(CancellationToken cancellationToken);
}