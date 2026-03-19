using Microsoft.EntityFrameworkCore;
using WebApp.Application.Gasolinas.Queries.GetGasolinas;
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
        return await _context.Gasolinas.AsNoTracking().AnyAsync(g => g.GasolinaId == gasolinaId, cancellationToken);

    }

    public async Task<List<GetGasolinasResponse>?> GetGasolinasListAsync(CancellationToken cancellationToken)
    {
        var gasolinas = await _context.Gasolinas
            .Select(g => new GetGasolinasResponse
            {
                Id = g.GasolinaId,
                Nombre = g.Nombre!
            })
            .ToListAsync(cancellationToken);

        return gasolinas;
    }
    public async Task<List<GetGasolinasWithPrecioResponse>?> GetGasolinasWithPrecioListAsync(CancellationToken cancellationToken)
    {
        var gasolinas = await _context.Gasolinas
            .Select(g => new GetGasolinasWithPrecioResponse
            {
                Id = g.GasolinaId,
                Nombre = g.Nombre!,
                Precio = g.GasolinaPrecios!.OrderByDescending(p => p.Fecha).Select(p => p.Precio).FirstOrDefault()
            })
            .ToListAsync(cancellationToken);

        return gasolinas;
    }

    public async Task<string?> GetNombreByIdAsync(int gasolinaId, CancellationToken cancellationToken)
    {
        return await _context.Gasolinas.Where(x=>x.GasolinaId == gasolinaId).Select(x=>x.Nombre).FirstOrDefaultAsync();
    }
}