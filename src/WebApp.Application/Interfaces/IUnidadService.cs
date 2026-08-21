using WebApp.Application.Unidades.Queries.GetUnidades;
using WebApp.Domain.Unidades;

namespace WebApp.Application.Interfaces;

public interface IUnidadService
{
    Task<List<GetUnidadesResponse>> GetUnidadesAsync();
    Task<UnidadesEnum?> GetUnidadIdByUsuarioIdAsync(int usuarioId, CancellationToken cancellationToken);
    Task<bool> UnidadesExistAsync(List<int> unidades, CancellationToken cancellationToken);
}