using WebApp.Domain;

namespace WebApp.Application.Interfaces;

public interface INombramientoPolicy
{
    Task<bool> NombramientosEstanAsignadosAsync(List<int> usuariosNombrados, CancellationToken cancellationToken);
    Task<bool> UsuarioEstaNombradoAsync(int usuarioId, CancellationToken cancellationToken);
}