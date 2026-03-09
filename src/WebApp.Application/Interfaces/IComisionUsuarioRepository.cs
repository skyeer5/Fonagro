using WebApp.Application.Core;
using WebApp.Domain;

namespace WebApp.Application.Interfaces;

public interface IComisionUsuarioRepository
{
    Task<Result<int>> UpdateAsync(ComisionUsuario comisionUsuario, CancellationToken cancellationToken);
}