using WebApp.Application.Core;
using WebApp.Application.Interfaces;
using WebApp.Domain;
using WebApp.Persistence;

namespace WebApp.Infrastructure.Repositories;

public class GasolinaPrecioRepository : IGasolinaPrecioRepository
{
    private readonly WebAppDbContext _context;
    private readonly ICurrentUser _currentUser;

    public GasolinaPrecioRepository(WebAppDbContext context, ICurrentUser currentUser)
    {
        _context = context;
        _currentUser = currentUser;
    }

    public async Task<Result<int>> CreateAsync(int gasolinaId, decimal precio, CancellationToken cancellationToken)
    {
        var userId = _currentUser.userId;
        var entity = new GasolinaPrecio
        {
            GasolinaId = gasolinaId,
            Precio = precio,
            Fecha = DateTime.UtcNow,
            Creado_Por = userId
        };

        _context.GasolinaPrecios.Add(entity);
        var resultado = await _context.SaveChangesAsync(cancellationToken);
        if(resultado <= 0)
        {
            return Result<int>.Failure("Error al crear el precio de gasolina");
        }
        return Result<int>.Success(resultado);
    }
}