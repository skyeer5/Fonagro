using WebApp.Application.Gasolinas.Queries.GetGasolinas;

namespace WebApp.Application.Interfaces;

public interface IGasolinaService
{
    Task<bool> GasolinaExistsAsync(int gasolinaId, CancellationToken cancellationToken);
    Task<string?> GetNombreByIdAsync(int gasolinaId, CancellationToken cancellationToken);
    Task<List<GetGasolinasResponse>?> GetGasolinasListAsync(CancellationToken cancellationToken);
}