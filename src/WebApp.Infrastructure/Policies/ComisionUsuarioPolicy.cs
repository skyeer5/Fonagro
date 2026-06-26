using Microsoft.EntityFrameworkCore;
using WebApp.Application.Interfaces;
using WebApp.Domain;
using WebApp.Persistence;

namespace WebApp.Infrastructure.Policies;

public class ComisionUsuarioPolicy : IComisionUsuarioPolicy
{
    private readonly WebAppDbContext _context;

    public ComisionUsuarioPolicy(WebAppDbContext context)
    {
        _context = context;
    }

    public async Task<bool> UsuariosEstanAsignadosAsync(List<Nombramiento> usuariosNombrados, CancellationToken cancellationToken)
    {
        var usuarioIds = usuariosNombrados.Select(u => u.UsuariosId).ToList();

        return await _context.ComisionUsuarios.AnyAsync(u=>
                usuarioIds.Contains(u.UsuarioId) 
                && u.Estado == WebApp.Domain.EstadosTipos.Asignado 
                , cancellationToken);
    }
}