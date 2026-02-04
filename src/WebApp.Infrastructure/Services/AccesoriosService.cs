using Microsoft.EntityFrameworkCore;
using WebApp.Application.Accesorios.Queries.GetAccesorios;
using WebApp.Application.Interfaces;
using WebApp.Persistence;

namespace WebApp.Infrastructure.Services;

public class AccesoriosService : IAccesoriosService
{
    private readonly WebAppDbContext _context;

    public AccesoriosService(WebAppDbContext context)
    {
        _context = context;
    }

    public async Task<List<GetAccesoriosResponse>> GetAccesoriosListAsync(CancellationToken cancellationToken)
    {
        return await _context.Accesorios
            .Where(a => a.Activo)
            .Select(a => new GetAccesoriosResponse
            {
                id = a.AccesorioId,
                Nombre = a.Nombre!
            })
            .ToListAsync(cancellationToken);
    }
}