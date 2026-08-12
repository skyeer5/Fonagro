using Microsoft.EntityFrameworkCore;
using WebApp.Application.Interfaces;
using WebApp.Domain;
using WebApp.Persistence;
using WebApp.Domain.Nombramientos;

namespace WebApp.Infrastructure.Policies;

public class NombramientoPolicy : INombramientoPolicy
{
    private readonly WebAppDbContext _context;

    public NombramientoPolicy(WebAppDbContext context)
    {
        _context = context;
    }

    public async Task<bool> UsuarioEstaNombradoAsync(int usuarioId, CancellationToken cancellationToken)
    {
        return await _context.Nombramientos.AnyAsync( u=>
            u.AsignacionUsuarioId == usuarioId &&
            u.Estado == NombramientoEstados.Creado,
            cancellationToken
        );
    }

    public async Task<bool> NombramientosEstanAsignadosAsync(List<int> usuariosNombrados, CancellationToken cancellationToken)
    {
        return await _context.Nombramientos.AnyAsync(u=>
                usuariosNombrados.Contains(u.AsignacionUsuarioId) 
                && (u.ComisionId != null)
                , cancellationToken);
    }

}