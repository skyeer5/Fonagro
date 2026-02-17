using Microsoft.EntityFrameworkCore;
using WebApp.Application.Comisiones.Queries.GetComisionesActivas;
using WebApp.Application.Interfaces;
using WebApp.Persistence;

namespace WebApp.Infrastructure.Services;

public class ComisionService : IComisionService
{
    private readonly WebAppDbContext _context;

    public ComisionService(WebAppDbContext context)
    {
        _context = context;
    }

    public async Task<List<GetComisionesActivasResponse>> GetComisionesActivasListAsync(int usuarioId)
    {
        return await _context.Comisiones
                .AsNoTracking()
                .Where(c => c.ComisionUsuarios!
                    .Any(cu => cu.UsuarioId == usuarioId))
                .Select(c => new GetComisionesActivasResponse
                {
                    id = c.ComisionId,
                    Departamento = c.Departamento,
                    Fecha_Salida = c.Fecha_Salida,
                    Fecha_Regreso = c.Fecha_Regreso,
                    Estado = c.Estado,

                    Nombramiento = c.ComisionUsuarios!
                        .First(cu => cu.UsuarioId == usuarioId).Nombramiento,

                    Piloto = c.ComisionUsuarios!
                        .First(cu => cu.UsuarioId == usuarioId).Es_Piloto
                })
                .ToListAsync();


    }
}