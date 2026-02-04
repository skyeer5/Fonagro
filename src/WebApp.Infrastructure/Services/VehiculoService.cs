using Microsoft.EntityFrameworkCore;
using WebApp.Application.Interfaces;
using WebApp.Domain;
using WebApp.Persistence;

namespace WebApp.Infrastructure.Services;

public class VehiculoService : IVehiculoService
{
    private readonly WebAppDbContext _context;

    public VehiculoService(WebAppDbContext context)
    {
        _context = context;
    }

    public Task<List<Vehiculo>> GetVehiculosDisponiblesAsync(CancellationToken cancellationToken)
    {
        return _context.Vehiculos
            .Where(v => v.Estado == EstadosTipos.Disponible) 
            .ToListAsync(cancellationToken);
    }

    public Task<bool> VehiculoExistsAsync(int vehiculoId, CancellationToken cancellationToken)
    {
        return _context.Vehiculos.AnyAsync(x => x.VehiculoId == vehiculoId, cancellationToken);
    }

    public Task<Vehiculo?> GetVehiculoByIdAsync(int vehiculoId, CancellationToken cancellationToken)
    {
        return _context.Vehiculos.FirstOrDefaultAsync(x => x.VehiculoId == vehiculoId, cancellationToken);
    }
}