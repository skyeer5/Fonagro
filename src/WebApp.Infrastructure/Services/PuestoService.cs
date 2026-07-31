using Microsoft.EntityFrameworkCore;
using WebApp.Application.Puestos.Queries.GetPuestosByUnidad;
using WebApp.Persistence;

namespace WebApp.Application.Interfaces;

public class PuestoService : IPuestoService
{
    private readonly WebAppDbContext _context;

    public PuestoService(WebAppDbContext context)
    {
        _context = context;
    }

    public async Task<bool> ExistsAsync(int puestoId)
    {
        return _context.Puestos.Any(x=>x.PuestoId == puestoId);
    }

    public async Task<List<GetPuestosByUnidadResponse>> GetPuestosByUnidadAsync(int unidadId)
    {
        return await _context.Puestos.Where(x=>x.UnidadId == unidadId).Select(x=> new GetPuestosByUnidadResponse
        {
            PuestoId = x.PuestoId,
            Nombre = x.Nombre,
        }).ToListAsync();
    }
}