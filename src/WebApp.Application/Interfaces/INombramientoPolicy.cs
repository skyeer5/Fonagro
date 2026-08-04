using WebApp.Domain;

namespace WebApp.Application.Interfaces;

public interface INombramientoPolicy
{
    Task<bool> UsuariosEstanAsignadosAsync(List<UsuariosNombrados> usuariosNombrados, CancellationToken cancellationToken);
    Task<bool> UsuarioEstaNombradoAsync(int usuarioId, CancellationToken cancellationToken);
}