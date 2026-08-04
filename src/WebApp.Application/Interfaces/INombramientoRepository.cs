using WebApp.Application.Nombramientos.Command.NombramientoCreate;

namespace WebApp.Application.Interfaces;

public interface INombramientoRepository
{
    Task<int> CreateNombramientoAsync(NombramientoCreateRequest request, CancellationToken cancellationToken);
}