using WebApp.Application.Nombramientos.Command.NombramientoCreate;
using WebApp.Domain.Unidades;

namespace WebApp.Application.Interfaces;

public interface INombramientoRepository
{
    Task<int> CreateNombramientoAsync(NombramientoCreateRequest request, int correlativo, CancellationToken cancellationToken);
    Task<int> ObtenerCorrelativoByUsuarioIdAsync(UnidadesEnum unidad, CancellationToken cancellationToken);
    Task<int> AprobarNombramientoAsync(int nombramientoId, CancellationToken cancellationToken);
    // Task CompletarNombramientoStatusAsync(CancellationToken cancellationToken);
    Task CancelarNombramientoStatusAsync(CancellationToken cancellationToken);
}