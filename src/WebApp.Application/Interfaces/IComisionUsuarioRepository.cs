namespace WebApp.Application.Interfaces;

public interface IComisionUsuarioRepository
{
    Task<bool> AddListAsync(List<int> usuariosIds, int comisionId, CancellationToken cancellationToken);
}