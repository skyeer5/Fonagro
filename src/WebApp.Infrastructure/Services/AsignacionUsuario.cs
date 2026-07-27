using Microsoft.EntityFrameworkCore;
using WebApp.Application.Interfaces;
using WebApp.Persistence;

namespace WebApp.Infrastructure.Services;

public class AsignacionUsuarioService : IAsignacionUsuarioService
{
    private readonly WebAppDbContext _context;

    public AsignacionUsuarioService(WebAppDbContext context)
    {
        _context = context;
    }

    public async Task<int?> GetAsignacionUsuarioIdByUsuarioIdAsync(int usuarioId, CancellationToken cancellationToken)
    {
        var asignacionUsuario = await _context.AsignacionesUsuarios!
            .Where(au => au.UsuarioId == usuarioId && au.Fecha_Desasignacion == null)
            .FirstOrDefaultAsync(cancellationToken);

        return asignacionUsuario?.UsuarioPuestoId;
    }
}