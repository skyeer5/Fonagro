using WebApp.Application.Core;
using WebApp.Application.Usuarios.Commands.UsuarioCreate;

namespace WebApp.Application.Interfaces;

public interface IUsuarioRepository
{
    Task<Result<int>> CreateUsuarioAsync(UsuarioCreateRequest request, CancellationToken cancellationToken);
}