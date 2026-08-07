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

    public async Task<bool> UsuariosEstanAsignadosAsync(List<UsuariosNombrados> usuariosNombrados, CancellationToken cancellationToken)
    {
        var usuarioIds = usuariosNombrados.Select(u => u.UsuariosId).ToList();

        return await _context.Nombramientos.AnyAsync(u=>
                /*usuarioIds.Contains(u.UsuarioPuesto!.UsuarioId) 
                &&*/ u.Estado == NombramientoEstados.Creado 
                , cancellationToken);
    }
}