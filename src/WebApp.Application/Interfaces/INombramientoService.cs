using WebApp.Application.Core;
using WebApp.Application.Nombramientos.Queries.GetNombramientos;
using WebApp.Domain.Nombramientos;

namespace WebApp.Application.Interfaces;

public interface INombramientoService
{
    Task<Nombramiento?> GetNMByIdComisionAndUsuarioIdAsync(int comisionId, CancellationToken cancellationToken); // Nombramiento(NM)
    Task<string?> GetDescripcionAsync(int comisionId, int usuarioId, CancellationToken cancellationToken); // Nombramiento(NM)
    Task<PagedList<GetNombramientosResponse>> GetNombramientosAsync(GetNombramientosRequest request,CancellationToken cancellationToken);
}