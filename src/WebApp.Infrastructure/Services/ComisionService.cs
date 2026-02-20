using System.Reflection;
using Microsoft.EntityFrameworkCore;
using WebApp.Application.ComisionDestinos.Queries.GetComisionDestinos;
using WebApp.Application.Comisiones.Queries.GetComisionesActivas;
using WebApp.Application.Interfaces;
using WebApp.Domain;
using WebApp.Persistence;

namespace WebApp.Infrastructure.Services;

public class ComisionService : IComisionService
{
    private readonly WebAppDbContext _context;

    public ComisionService(WebAppDbContext context)
    {
        _context = context;
    }

    public async Task<GetComisionActivaResponse?> GetComisionActivaAsync(int usuarioId)
    {
        return await _context.Comisiones
                .AsNoTracking()
                .Where(c => c.ComisionUsuarios!
                    .Any(cu => cu.UsuarioId == usuarioId))
                .Select(c => new GetComisionActivaResponse
                {
                    id = c.ComisionId,
                    Departamento = c.Departamento,
                    Fecha_Salida = c.Fecha_Salida,
                    Fecha_Regreso = c.Fecha_Regreso,
                    Estado = c.Estado,

                    Nombramiento = c.ComisionUsuarios!
                        .First(cu => cu.UsuarioId == usuarioId).Nombramiento,

                    Piloto = c.ComisionUsuarios!
                        .First(cu => cu.UsuarioId == usuarioId).Es_Piloto,
                    destinos = c.ComisionDestinos!
                                    .Select( x=> new GetComisionDestinosResponse
                                    {
                                        id = x.ComisionDestinoId,
                                        descripcion =x.Descripcion,
                                        kilometros = x.Kilometros
                                    }).ToList()
                })
                .FirstOrDefaultAsync();


    }

    public async Task<Comision?> GetComisionByIdAsync(int comisionId)
    {
       return await _context.Comisiones.Where(x=>x.ComisionId == comisionId).Include(x=>x.Vehiculo).FirstOrDefaultAsync();
    }

}