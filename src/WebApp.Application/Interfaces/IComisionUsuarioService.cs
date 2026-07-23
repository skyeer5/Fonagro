using WebApp.Domain.Nombramientos;

namespace WebApp.Application.Interfaces;

public interface IComisionUsuarioService
{
    Task<Nombramiento?> GetCUByIdComisionAndUsuarioIdAsync(int comisionId, CancellationToken cancellationToken); // ComisionUsuario(CU)
    Task<string?> GetDescripcionAsync(int comisionId, int usuarioId, CancellationToken cancellationToken); // ComisionUsuario(CU)
}