using WebApp.Application.Core;
using WebApp.Application.Interfaces;
using WebApp.Domain;
using WebApp.Persistence;

namespace WebApp.Infrastructure.Repositories;

public class VehiculoRepository : IVehiculoRepository
{
    private readonly WebAppDbContext _context;
    private readonly ICurrentUser _currentUser;

    public VehiculoRepository(WebAppDbContext context, ICurrentUser currentUser)
    {
        _context = context;
        _currentUser = currentUser;
    }

    public async Task<Result<int>> CreateVehiculoAsync(Vehiculo vehiculo, CancellationToken cancellationToken)
    {
        var userId = _currentUser.userId;
        vehiculo.AgregarCreadoPor(userId);

        await _context.Vehiculos.AddAsync(vehiculo);
        var result = await _context.SaveChangesAsync(cancellationToken);
        return result > 0 ? Result<int>.Success(result) : Result<int>.Failure("Error al crear el vehículo");
    }
}