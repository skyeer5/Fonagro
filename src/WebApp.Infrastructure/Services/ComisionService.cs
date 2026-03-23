using System.Reflection;
using Bogus;
using Microsoft.EntityFrameworkCore;
using WebApp.Application.ComisionDestinos.Queries.GetComisionDestinos;
using WebApp.Application.ComisionDestinos.Queries.GetComisionDestinosDetail;
using WebApp.Application.Comisiones.Queries.GetComisionesActivas;
using WebApp.Application.Comisiones.Queries.GetComisionesPendApprov;
using WebApp.Application.Comisiones.Queries.PlanViajeExcel;
using WebApp.Application.ComisionViaticos.Queries.GetComisionViatico;
using WebApp.Application.Core;
using WebApp.Application.Interfaces;
using WebApp.Domain;
using WebApp.Persistence;

namespace WebApp.Infrastructure.Services;

public class ComisionService : IComisionService
{
    private readonly WebAppDbContext _context;
    private readonly ICurrentUser _currentUser;

    public ComisionService(WebAppDbContext context, ICurrentUser currentUser)
    {
        _context = context;
        _currentUser = currentUser;
    }

    public async Task<GetComisionActivaResponse?> GetComisionActivaAsync()
    {
        var userId = _currentUser.userId;
        return await _context.Comisiones
                .AsNoTracking()
                .Where(c => 
                    c.Estado!= EstadosTipos.Cancelada 
                    && c.Estado != EstadosTipos.Completada 
                    && c.ComisionUsuarios! 
                    .Any(cu => cu.UsuarioId == userId && cu.Estado != EstadosTipos.Cancelada && cu.Estado !=EstadosTipos.Completada))
                .Select(c => new GetComisionActivaResponse
                {
                    id = c.ComisionId,
                    Departamento = c.Departamento,
                    Fecha_Salida = c.Fecha_Salida,
                    Fecha_Regreso = c.Fecha_Regreso,
                    Estado = c.Estado,
                    Descripcion = c.ComisionUsuarios!
                        .First(cu => cu.UsuarioId == userId).Descripcion,
                    Prespuesto_Aprobado = c.Presupuesto_Combustible_Aprobado != 0,
                    Nombramiento = c.ComisionUsuarios!
                        .First(cu => cu.UsuarioId == userId).Nombramiento,

                    Piloto = c.ComisionUsuarios!
                        .First(cu => cu.UsuarioId == userId).Es_Piloto,
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

    public async Task<List<GetComisionesPendApprovResponse>?> GetComisionPendApprovAsync()
    {
         return await _context.Comisiones
                .Where(c => c.Estado == EstadosTipos.DestinosDefinidos)
                .Select(c => new GetComisionesPendApprovResponse
                {
                    id = c.ComisionId,
                    Departamento = c.Departamento,
                    Fecha_Salida = c.Fecha_Salida,
                    Fecha_Regreso = c.Fecha_Regreso,
                    Prespuesto_Estimado = c.Presupuesto_Combustible_Estimado
                })
                .ToListAsync();
    }

    public async Task<Result<PlanViajeResponse>> GetPlanViajeResponseAsync(int idUsuario, int idComision)
    {
            var planViaje = await _context.Comisiones
            .Where(p => p.ComisionId == idComision)
            .Select(p => new PlanViajeResponse
            {
                Departamento = p.Departamento,
                Fecha_Salida = p.Fecha_Salida,
                Fecha_Regreso = p.Fecha_Regreso,
                Descripcion = p.ComisionUsuarios!.FirstOrDefault(cu => cu.UsuarioId == idUsuario)!.Descripcion,
                TotalCombustibleAutorizado = p.Presupuesto_Combustible_Aprobado,
                Es_Gasolina = p.Vehiculo!.Gasolina!.Nombre != GasolinaTipos.Disel ? true : false,
                Precio_Galon = p.Vehiculo!.Gasolina!.GasolinaPrecios!
                    .OrderByDescending(gp => gp.Fecha)
                    .Select(gp => gp.Precio)
                    .FirstOrDefault(),
                Viaticos = p.ComisionUsuarios!
                    .Where(cu => cu.UsuarioId == idUsuario)
                    .SelectMany(cu => cu.ComisionViaticosList!)
                    .Select(v => new GetComisionViaticoResponse
                    {
                        Tipo_viatico = v.Viatico!.Nombre,
                        Monto = v.Viatico.Monto,
                        Fecha = DateOnly.FromDateTime(v.Fecha)
                    })
                    .ToList(),
                Destinos = p.ComisionDestinos!
                    .Select(d => new GetComisionDestinosDetailResponse
                    {
                        Descripcion = d.Descripcion,
                        Kilometros = d.Kilometros,
                        Galones = d.Galones,
                    })
                    .ToList()
            }).FirstOrDefaultAsync();
        if(planViaje is null)
        {
            Console.WriteLine("\n\n\n Es nuloooo\n\n\n");
            return Result<PlanViajeResponse>.Failure("Error al encontrar el plan de viaje");
        }
            Console.WriteLine("\n\n\n Si paasaaaa\n\n\n");
        return Result<PlanViajeResponse>.Success(planViaje);
    }
}