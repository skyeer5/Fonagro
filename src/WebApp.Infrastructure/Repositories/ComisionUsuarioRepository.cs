using WebApp.Application.Core;
using WebApp.Application.Interfaces;
using WebApp.Domain;
using WebApp.Persistence;

namespace WebApp.Infrastructure.Repositories;

public class ComisionUsuarioRepository : IComisionUsuarioRepository
{
    private readonly WebAppDbContext _context;

    public ComisionUsuarioRepository(WebAppDbContext context)
    {
        _context = context;
    }

    public async Task<Result<int>> UpdateAsync(ComisionUsuario comisionUsuario, CancellationToken cancellationToken)
    {
        _context.Entry(comisionUsuario).State = Microsoft.EntityFrameworkCore.EntityState.Modified;

        var resultado = await _context.SaveChangesAsync(cancellationToken);

        return resultado > 0 ? Result<int>.Success(resultado) : Result<int>.Failure("Error al modificar el plan de viaje");
    }
}