using WebApp.Application.Core;
using WebApp.Domain;

namespace WebApp.Application.Interfaces;

public interface IGasolinaService
{
    Task<bool> GasolinaExistsAsync(int gasolinaId, CancellationToken cancellationToken);
    Task<string?> GetNombreByIdAsync(int gasolinaId, CancellationToken cancellationToken);
}