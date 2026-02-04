using WebApp.Application.Core;
using WebApp.Application.Interfaces;
using WebApp.Domain;
using WebApp.Persistence;

namespace WebApp.Infrastructure.Repositories;

public class VehiculoRepository : IVehiculoRepository
{
    private readonly WebAppDbContext _context;

    public VehiculoRepository(WebAppDbContext context)
    {
        _context = context;
    }

    public async Task<Result<int>> CreateVehiculoAsync(Vehiculo vehiculo, CancellationToken cancellationToken)
    {
        await _context.Vehiculos.AddAsync(vehiculo);
        var result = await _context.SaveChangesAsync(cancellationToken);
        return result > 0 ? Result<int>.Success(result) : Result<int>.Failure("Error al crear el vehículo");
    }
}