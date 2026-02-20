using WebApp.Application.Core;
using WebApp.Application.Interfaces;
using WebApp.Domain;
using WebApp.Persistence;

namespace WebApp.Infrastructure.Repositories;

public class ComisionRepository : IComisionRepository
{
    private readonly WebAppDbContext _context;

    public ComisionRepository(WebAppDbContext context)
    {
        _context = context;
    }

    public async Task<Result<int>> AddAsync(Domain.Comision comision, CancellationToken cancellationToken)
    {
        await _context.Comisiones.AddAsync(comision, cancellationToken);
        var result = await _context.SaveChangesAsync(cancellationToken);
        return result > 0 ? Result<int>.Success(comision.ComisionId) : Result<int>.Failure("Error al agregar la comisión");
    }

    public async Task<Result<int>> AddDestinosAsync(Comision comision, CancellationToken cancellationToken)
    {
        _context.Entry(comision).State = Microsoft.EntityFrameworkCore.EntityState.Modified;

        var resultado = await _context.SaveChangesAsync(cancellationToken);

        return resultado > 0 ? Result<int>.Success(resultado) : Result<int>.Failure("Error al agregar los destinos de la comisión");
    }
}