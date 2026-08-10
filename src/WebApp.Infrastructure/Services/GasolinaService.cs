using Microsoft.EntityFrameworkCore;
using WebApp.Application.Gasolinas.Queries.GetGasolinas;
using WebApp.Application.Gasolinas.Queries.GetGasolinasWithFecha;
using WebApp.Application.Gasolinas.Queries.GetGasolinasWithPrecio;
using WebApp.Application.Interfaces;
using WebApp.Persistence;

namespace WebApp.Infrastructure.Services;

public class GasolinaService : IGasolinaService
{
    private readonly WebAppDbContext _context;
    public GasolinaService(WebAppDbContext context)
    {
        _context = context;
    }
    public async Task<bool> GasolinaExistsAsync(int gasolinaId, CancellationToken cancellationToken)
    {
        return await _context.Combustibles.AsNoTracking().AnyAsync(g => g.CombustibleId == gasolinaId, cancellationToken);

    }

    public async Task<List<GetGasolinasResponse>?> GetGasolinasListAsync(CancellationToken cancellationToken)
    {
        var gasolinas = await _context.Combustibles
            .Select(g => new GetGasolinasResponse
            {
                Id = g.CombustibleId,
                Nombre = g.Nombre!
            })
            .ToListAsync(cancellationToken);

        return gasolinas;
    }
    public async Task<List<GetGasolinasWithPrecioResponse>?> GetGasolinasWithPrecioListAsync(CancellationToken cancellationToken)
    {
        var gasolinas = await _context.Combustibles
            .Select(g => new GetGasolinasWithPrecioResponse
            {
                Id = g.CombustibleId,
                Nombre = g.Nombre!,
                Precio = g.GasolinaPrecios!.OrderByDescending(p => p.Fecha).Select(p => p.Precio).FirstOrDefault()
            })
            .ToListAsync(cancellationToken);

        return gasolinas;
    }
    public async Task<List<GetGasolinasWithFechaResponse>?> GetGasolinasWithFechaListAsync(CancellationToken cancellationToken)
    {
        var gasolinas = await _context.Combustibles
            .Select(g =>GetGasolinasWithFechaResponse.Crear
            (
                g.Nombre!,
                g.GasolinaPrecios!.OrderByDescending(p => p.Fecha).Select(p => p.Precio).FirstOrDefault(),
                g.GasolinaPrecios!.OrderByDescending(p => p.Fecha).Select(p => p.Fecha).FirstOrDefault()
            ))
            .ToListAsync(cancellationToken);

        return gasolinas;
    }

    public async Task<string?> GetNombreByIdAsync(int gasolinaId, CancellationToken cancellationToken)
    {
        return await _context.Combustibles.Where(x=>x.CombustibleId == gasolinaId).Select(x=>x.Nombre).FirstOrDefaultAsync();
    }
}