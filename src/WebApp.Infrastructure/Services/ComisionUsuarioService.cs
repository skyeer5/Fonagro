using Microsoft.EntityFrameworkCore;
using WebApp.Application.Interfaces;
using WebApp.Domain.Nombramientos;
using WebApp.Persistence;

namespace WebApp.Infrastructure.Services;

public class ComisionUsuarioService : IComisionUsuarioService
{
    private readonly WebAppDbContext _context;
    private readonly ICurrentUser _currentUser;

    public ComisionUsuarioService(WebAppDbContext context, ICurrentUser currentUser)
    {
        _context = context;
        _currentUser = currentUser;
    }

    public async Task<Nombramiento?> GetCUByIdComisionAndUsuarioIdAsync(int comisionId, CancellationToken cancellationToken)
    {
        var userId = _currentUser.userId;
        return await _context.Nombramientos!.Where(cu=>cu.ComisionId == comisionId && cu.UsuarioId == userId)
                                        .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<string?> GetDescripcionAsync(int comisionId, int usuarioId, CancellationToken cancellationToken)
    {
        return await _context.Nombramientos!.Where(cu=>cu.ComisionId == comisionId && cu.UsuarioId == usuarioId)
                                        .Select(cu => cu.Descripcion)
                                        .FirstOrDefaultAsync(cancellationToken);
    }
}