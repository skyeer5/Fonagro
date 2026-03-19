using WebApp.Application.Core;

namespace WebApp.Application.Interfaces;

public interface IGasolinaPrecioRepository
{
    Task<Result<int>> CreateAsync(int gasolinaId, decimal precio, CancellationToken cancellationToken);
}