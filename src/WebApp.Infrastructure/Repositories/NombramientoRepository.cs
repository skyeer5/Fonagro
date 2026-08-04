using WebApp.Application.Core;
using WebApp.Application.Interfaces;
using WebApp.Application.Nombramientos.Command.NombramientoCreate;
using WebApp.Domain.Nombramientos;
using WebApp.Persistence;

namespace WebApp.Infrastructure.Repositories;

public class NombramientoRepository : INombramientoRepository
{
    private readonly WebAppDbContext _context;

    public NombramientoRepository(WebAppDbContext context)
    {
        _context = context;
    }

    public async Task<int> CreateNombramientoAsync(NombramientoCreateRequest request, CancellationToken cancellationToken)
    {
        var nombramiento = Nombramiento.Crear(request.UsuarioId, request.Proposito!, request.Fecha_Salida, request.Fecha_Regreso, request.Municipios!);
        await _context.Nombramientos.AddAsync(nombramiento);
        var result = await _context.SaveChangesAsync(cancellationToken);
        return result > 0 ? nombramiento.NombramientoId : result;
    }
}