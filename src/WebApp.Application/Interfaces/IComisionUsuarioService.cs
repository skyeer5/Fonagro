using WebApp.Domain;

namespace WebApp.Application.Interfaces;

public interface IComisionUsuarioService
{
    Task<ComisionUsuario?> GetCUByIdComisionAndUsuarioIdAsync(int comisionId, int usuarioId, CancellationToken cancellationToken); // ComisionUsuario(CU)
    Task<string?> GetDescripcionAsync(int comisionId, int usuarioId, CancellationToken cancellationToken); // ComisionUsuario(CU)
}