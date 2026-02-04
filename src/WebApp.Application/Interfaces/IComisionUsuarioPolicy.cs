using WebApp.Domain;

namespace WebApp.Application.Interfaces;

public interface IComisionUsuarioPolicy
{
    Task<bool> UsuariosEstanAsignadosAsync(List<Nombramiento> usuariosNombrados, CancellationToken cancellationToken);
}