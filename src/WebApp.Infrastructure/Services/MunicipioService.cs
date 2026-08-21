using Microsoft.EntityFrameworkCore;
using WebApp.Application.Interfaces;
using WebApp.Application.Municipios.Queries.GetMunicipiosByDep;
using WebApp.Persistence;

namespace WebApp.Infrastructure.Services;

public class MunicipioService : IMunicipioService
{
    private readonly WebAppDbContext _context;

    public MunicipioService(WebAppDbContext context)
    {
        _context = context;
    }

    public async Task<List<GetMunicipiosByDepResponse>> GetMunicipiosByDepAsync(int departamentoId)
    {
        return await _context.Municipios
            .Where(m => m.DepartamentoId == departamentoId)
            .Select(m => new GetMunicipiosByDepResponse
            {
                Id = m.MunicipioId,
                Nombre = m.Nombre
            }).ToListAsync();
    }

    public async Task<bool> MunicipiosExistsAsync(List<int> municipiosIds)
    {
        return await _context.Municipios.AnyAsync(m => municipiosIds.Contains(m.MunicipioId));
    }
}