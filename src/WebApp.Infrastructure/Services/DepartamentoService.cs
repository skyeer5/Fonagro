using Microsoft.EntityFrameworkCore;
using WebApp.Application.Departamentos.Queries.GetDepartamentos;
using WebApp.Application.Interfaces;
using WebApp.Persistence;

namespace WebApp.Infrastructure.Services;

public class DepartamentoService : IDepartamentoService
{
    private readonly WebAppDbContext _context;

    public DepartamentoService(WebAppDbContext context)
    {
        _context = context;
    }

    public async Task<bool> DepartamentosExistAsync(List<int> departamentosIds, CancellationToken cancellationToken)
    {
        return await _context.Departamentos.AnyAsync(x=>departamentosIds.Contains(x.DepartamentoId), cancellationToken);
    }

    public async Task<List<GetDepartamentosResponse>> GetDepartamentosAsync()
    {
        return await _context.Departamentos.Select(d => new GetDepartamentosResponse
        {
            Id = d.DepartamentoId,
            Nombre = d.Nombre
        }).ToListAsync();
    }
}