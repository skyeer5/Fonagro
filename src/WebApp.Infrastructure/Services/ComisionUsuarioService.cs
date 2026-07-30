using Microsoft.EntityFrameworkCore;
using WebApp.Application.Interfaces;
using WebApp.Domain.Nombramientos;
using WebApp.Persistence;

namespace WebApp.Infrastructure.Services;

public class NombramientoService : INombramientoService
{
    private readonly WebAppDbContext _context;
    private readonly ICurrentUser _currentUser;
    private readonly IAsignacionUsuarioService _asignacionUsuarioService;

    public NombramientoService(WebAppDbContext context, ICurrentUser currentUser, IAsignacionUsuarioService asignacionUsuarioService)
    {
        _context = context;
        _currentUser = currentUser;
        _asignacionUsuarioService = asignacionUsuarioService;
    }

    public async Task<Nombramiento?> GetNMByIdComisionAndUsuarioIdAsync(int comisionId, CancellationToken cancellationToken)
    {
        var userId = _currentUser.userId;
        var usuarioPuestoId = await _asignacionUsuarioService.GetAsignacionUsuarioIdByUsuarioIdAsync(userId, cancellationToken);
        return await _context.Nombramientos!.Where(cu=>cu.ComisionId == comisionId && cu.AsignacionUsuarioId == usuarioPuestoId)
                                        .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<string?> GetDescripcionAsync(int comisionId, int usuarioId, CancellationToken cancellationToken)
    {
        var usuarioPuestoId = await _asignacionUsuarioService.GetAsignacionUsuarioIdByUsuarioIdAsync(usuarioId, cancellationToken);

        return await _context.Nombramientos!.Where(cu=>cu.ComisionId == comisionId && cu.AsignacionUsuarioId == usuarioPuestoId)
                                        .Select(cu => cu.Descripcion)
                                        .FirstOrDefaultAsync(cancellationToken);
    }
}