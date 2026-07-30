using Microsoft.EntityFrameworkCore;
using WebApp.Application.Interfaces;
using WebApp.Application.Unidades.Queries.GetUnidades;
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
}