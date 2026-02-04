using Microsoft.EntityFrameworkCore;
using WebApp.Application.Interfaces;
using WebApp.Persistence;

namespace WebApp.Infrastructure.Services;

public class GasolinaPrecioService : IGasolinaPrecioService
{
    private readonly WebAppDbContext _context;
    public GasolinaPrecioService(WebAppDbContext context)
    {
        _context = context;
    }
    public async Task<decimal?> GetPrecioActualByVehiculoIdAsync(int vehiculoId, CancellationToken cancellationToken)
    {
        return await _context.Vehiculos
                .AsNoTracking()
                .Where(x=>x.VehiculoId == vehiculoId)
                .SelectMany(x=> _context.GasolinaPrecios
                    .Where(gp=>gp.GasolinaId==x.GasolinaId))
                    .OrderByDescending(gp => gp.Fecha)
                    .Select(gp => gp.Precio)
                    .Take(1)
                .FirstOrDefaultAsync(cancellationToken);
    }
}