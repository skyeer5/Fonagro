using WebApp.Application.Core;
using WebApp.Domain.Nombramientos;

namespace WebApp.Application.Interfaces;

public interface IComisionUsuarioRepository
{
    Task<Result<int>> UpdateAsync(Nombramiento comisionUsuario, CancellationToken cancellationToken);
}