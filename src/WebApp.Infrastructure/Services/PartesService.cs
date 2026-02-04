using Microsoft.EntityFrameworkCore;
using WebApp.Application.Interfaces;
using WebApp.Application.Partes.Queries.GetPartes;
using WebApp.Persistence;

namespace WebApp.Infrastructure.Services;

public class PartesService : IPartesService
{
    private readonly WebAppDbContext _context;

    public PartesService(WebAppDbContext context)
    {
        _context = context;
    }

    public async Task<List<GetPartesResponse>> GetPartesListAsync(CancellationToken cancellationToken)
    {
        return await _context.Partes
            .Where(p => p.Activo)
            .Select(p => new GetPartesResponse
            {
                id = p.ParteId,
                Nombre = p.Nombre!
            })
            .ToListAsync(cancellationToken);
    }
}