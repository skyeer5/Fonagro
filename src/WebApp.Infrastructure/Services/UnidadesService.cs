using Microsoft.EntityFrameworkCore;
using WebApp.Application.Interfaces;
using WebApp.Application.Unidades.Queries.GetUnidades;
using WebApp.Domain.Unidades;
using WebApp.Persistence;

namespace WebApp.Infrastructure.Services;

public class UnidadService : IUnidadService
{
    private readonly WebAppDbContext _context;
    public UnidadService(WebAppDbContext context)
    {
        _context = context;
    }

    public Task<List<GetUnidadesResponse>> GetUnidadesAsync()
    {
        return _context.Unidades
            .Select(u => new GetUnidadesResponse
            {
                Id = u.UnidadId,
                Nombre = u.Nombre
            })
            .ToListAsync();
    }

    public async Task<UnidadesEnum?> GetUnidadIdByUsuarioIdAsync(int usuarioId, CancellationToken cancellationToken)
    {
        var unidadId = await  _context.AsignacionesUsuarios
                .Include(p=>p.Puesto)
                .Where(x=>x.AsignacionUsuarioId == usuarioId)
                .Select(x=>x.Puesto!.UnidadId)
                .FirstOrDefaultAsync();
                
        return (UnidadesEnum?)unidadId;
    }
}