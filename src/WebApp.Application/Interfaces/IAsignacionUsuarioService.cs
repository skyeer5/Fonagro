namespace WebApp.Application.Interfaces;
public interface IAsignacionUsuarioService
{
    Task<int?> GetAsignacionUsuarioIdByUsuarioIdAsync(int usuarioId, CancellationToken cancellationToken);
}