using Microsoft.EntityFrameworkCore;
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
}