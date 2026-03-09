using Microsoft.EntityFrameworkCore;
using WebApp.Application.Interfaces;
using WebApp.Domain;
using WebApp.Persistence;

namespace WebApp.Infrastructure.Services;

public class ComisionUsuarioService : IComisionUsuarioService
{
    private readonly WebAppDbContext _context;

    public ComisionUsuarioService(WebAppDbContext context)
    {
        _context = context;
    }

    public async Task<ComisionUsuario?> GetCUByIdComisionAndUsuarioIdAsync(int comisionId, int usuarioId, CancellationToken cancellationToken)
    {
        return await _context.ComisionUsuarios!.Where(cu=>cu.ComisionId == comisionId && cu.UsuarioId == usuarioId)
                                        .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<string?> GetDescripcionAsync(int comisionId, int usuarioId, CancellationToken cancellationToken)
    {
        return await _context.ComisionUsuarios!.Where(cu=>cu.ComisionId == comisionId && cu.UsuarioId == usuarioId)
                                        .Select(cu => cu.Descripcion)
                                        .FirstOrDefaultAsync(cancellationToken);
    }
}