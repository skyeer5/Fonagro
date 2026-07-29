using System.Linq.Expressions;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using WebApp.Application.ComisionDestinos.Queries.GetComisionDestinos;
using WebApp.Application.ComisionDestinos.Queries.GetComisionDestinosDetail;
using WebApp.Application.Comisiones.Queries.GetComisionesActivas;
using WebApp.Application.Comisiones.Queries.GetComisionesDetalle;
using WebApp.Application.Comisiones.Queries.GetComisionesPendApprov;
using WebApp.Application.Comisiones.Queries.PlanViajeExcel;
using WebApp.Application.ComisionViaticos.Queries.GetComisionViatico;
using WebApp.Application.Core;
using WebApp.Application.Interfaces;
using WebApp.Domain.Nombramientos;
using WebApp.Domain.Gasolinas;
using WebApp.Persistence;
using WebApp.Domain.Comisiones;


namespace WebApp.Infrastructure.Services;

public class ComisionService : IComisionService
{
    private readonly WebAppDbContext _context;
    private readonly ICurrentUser _currentUser;
    private readonly IMapper _mapper;

    public ComisionService(WebAppDbContext context, ICurrentUser currentUser, IMapper mapper)
    {
        _context = context;
        _currentUser = currentUser;
        _mapper = mapper;
    }

    public async Task<GetComisionActivaResponse?> GetComisionActivaAsync()
    {
        var userId = _currentUser.userId;
        return await _context.Comisiones
                .AsNoTracking()
                .Where(c => 
                    c.Estado!= ComisionEstados.Cancelada 
                    && c.Estado != ComisionEstados.Completada 
                    /*&& c.Nombramientos! 
                    .Any(cu => cu.UsuarioId == userId && cu.Estado != NombramientoTipos.Cancelada && cu.Estado !=NombramientoTipos.Completada)*/)
                .Select(c => new GetComisionActivaResponse
                {
                    id = c.ComisionId,
                    // Departamento = c.Departamento,
                    Fecha_Salida = c.Fecha_Salida,
                    Fecha_Regreso = c.Fecha_Regreso,
                    Estado = c.Estado,
                    // Descripcion = c.Comision!
                    //     .First(cu => cu.UsuarioId == userId).Descripcion,
                    // Prespuesto_Aprobado = c.Presupuesto_Combustible_Aprobado != 0,
                    // Nombramiento = c.Nombramientos!
                    //     .First(cu => cu.UsuarioId == userId).Num_Nombramiento,

                    // Piloto = c.Nombramientos!
                    //     .First(cu => cu.UsuarioId == userId).Es_Piloto,
                    // Destinos = c.ComisionDestinos!
                    //                 .Select( x=> new GetComisionDestinosResponse
                    //                 {
                    //                     id = x.ComisionDestinoId,
                    //                     descripcion =x.Descripcion,
                    //                     kilometros = x.Kilometros
                    //                 }
                    
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
                .Where(c => c.Estado == ComisionEstados.DestinosDefinidos)
                .Select(c => new GetComisionesPendApprovResponse
                {
                    id = c.ComisionId,
                    Departamento = c.Departamento,
                    Fecha_Salida = c.Fecha_Salida,
                    Fecha_Regreso = c.Fecha_Regreso,
                    Kilometros = c.ComisionDestinos!.Sum(x=>x.Kilometros),
                    precio_Gasolina = c.Precio_Gasolina_Usado,
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
                // Descripcion = p.Nombramientos!.FirstOrDefault(cu => cu.UsuarioId == idUsuario)!.Descripcion,
                TotalCombustibleAutorizado = p.Presupuesto_Combustible_Aprobado,
                Es_Gasolina = p.Vehiculo!.Gasolina!.Nombre != GasolinaTipos.Disel ? true : false,
                Precio_Galon = p.Vehiculo!.Gasolina!.GasolinaPrecios!
                    .OrderByDescending(gp => gp.Fecha)
                    .Select(gp => gp.Precio)
                    .FirstOrDefault(),
                // Viaticos = p.Nombramientos!
                //     .Where(cu => cu.UsuarioId == idUsuario)
                //     .SelectMany(cu => cu.ComisionViaticosList!)
                //     .Select(v => new GetComisionViaticoResponse
                //     {
                //         Tipo_viatico = v.Viatico!.Nombre,
                //         Monto = v.Viatico.Monto,
                //         Fecha = DateOnly.FromDateTime(v.Fecha)
                //     })
                //     .ToList(),
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
    public async Task<Result<PagedList<GetComisionesDetalleResponse>>> GetComisionesDetalleAsync(GetComisionesDetalleRequest request)
    {
        IQueryable<Comision> queryable = _context.Comisiones.AsNoTracking();
        
        var predicate = ExpressionBuilder.New<Comision>();
        if(request.Fecha_Inicio is not null && request.Fecha_Fin is not null)
        {
            predicate = predicate.And(x=>
                            x.Fecha_Salida <= request.Fecha_Fin && x.Fecha_Regreso>=request.Fecha_Inicio 
                        );
        }
        if(!string.IsNullOrEmpty(request.Departamento))
        {
            predicate = predicate.And(x=>
                            x.Departamento!
                            .Contains(request.Departamento)
                        );
        }
        if(!string.IsNullOrEmpty(request.Estado))
        {
            predicate = predicate.And(x=>
                            x.Estado!
                            .Contains(request.Estado)
                        );
        }
        if(!string.IsNullOrEmpty(request.OrderBy))
        {
            Expression<Func<Comision, object>> orderBySelector =
                        request.OrderBy.ToLower() switch
                        {
                            "fecha_inicio" => com => com.Fecha_Salida,
                            "fecha_fin" => com => com.Fecha_Regreso,
                            "departamento" => com => com.Departamento!,
                            "estado" => com => com.Estado!,
                            _ => com => com.ComisionId
                        };
            bool orderBy = request.OrderAsc.HasValue
                            ? request.OrderAsc.Value
                            : true;
            queryable = orderBy ? queryable.OrderBy(orderBySelector) : queryable.OrderByDescending(orderBySelector);
        }
        queryable = queryable.Where(predicate);

        var comisionsQuery = queryable.ProjectTo<GetComisionesDetalleResponse>(_mapper.ConfigurationProvider).AsQueryable();
        var pagination = await PagedList<GetComisionesDetalleResponse>.CreateAsync(
                                    comisionsQuery,
                                    request.PageNumber,
                                    request.PageSize
        );
        return Result<PagedList<GetComisionesDetalleResponse>>.Success(pagination);
    }
}