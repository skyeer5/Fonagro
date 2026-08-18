using System.Linq.Expressions;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using WebApp.Application.ComisionDestinos.Queries.GetComisionDestinos;
using WebApp.Application.ComisionDestinos.Queries.GetComisionDestinosDetail;
using WebApp.Application.Comisiones.Queries.GetComisionesActivas;
using WebApp.Application.Comisiones.Queries.GetComisionesDetalle;
using WebApp.Application.Comisiones.Queries.GetComisionesPendApprov;
using WebApp.Application.Comisiones.Queries.PlanViajePdf;
using WebApp.Application.ComisionViaticos.Queries.GetComisionViatico;
using WebApp.Application.Core;
using WebApp.Application.Interfaces;
using WebApp.Domain.Nombramientos;
using WebApp.Domain.Gasolinas;
using WebApp.Persistence;
using WebApp.Domain.Comisiones;
using WebApp.Domain.Unidades;


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
        var query = from n in _context.Nombramientos.AsNoTracking()
                    join c in _context.Comisiones
                            on n.ComisionId equals c.ComisionId
                    join au in _context.AsignacionesUsuarios 
                            on n.AsignacionUsuarioId equals au.AsignacionUsuarioId
                    join u in _context.Users
                            on au.UsuarioId equals u.Id
                    where u.Id == userId 
                        && au.Fecha_Desasignacion == null
                        && c.Estado != ComisionEstados.Cancelada 
                        && c.Estado != ComisionEstados.Completada
                    select new GetComisionActivaResponse
                    {
                        ComisionId = c.ComisionId,
                        Departamento = string.Join(", ", n.NomMunicipios!.Select(nm=>nm.Municipio!.Departamento.Nombre)),
                        Municipio = string.Join(", ", n.NomMunicipios!.Select(nm=>nm.Municipio!.Nombre)),
                        Fecha_Salida = n.Fecha_Salida,
                        Fecha_Regreso = n.Fecha_Regreso,
                        Estado = c.Estado,
                        Descripcion = n.Descripcion,
                        Prespuesto_Aprobado = c.Presupuesto_Combustible_Aprobado != 0,
                        Nombramiento = $"FON-{((UnidadesEnum)n.AsignacionUsuario!.Puesto!.UnidadId).ToString()}-{n.Correlativo}-{n.Fecha_Creado.Year}",
                        Piloto = n.NombramientoId == c.NombramientoId_Respon_Vehiculo,
                        Destinos = c.ComisionDestinos!
                                        .Select( cd=> new GetComisionDestinosResponse
                                        {
                                            id = cd.ComisionDestinoId,
                                            descripcion = cd.Descripcion,
                                            kilometros = cd.Kilometros
                                        }).ToList()
                    };
        return await query.FirstOrDefaultAsync();                

    }

    public async Task<Comision?> GetComisionByIdAsync(int comisionId)
    {
       return await _context.Comisiones.Where(x=>x.ComisionId == comisionId).Include(x=>x.Vehiculo).FirstOrDefaultAsync();
    }

    public async Task<List<GetComisionesPendApprovResponse>?> GetComisionPendApprovAsync()
    {
        var comisiones = await _context.Comisiones
        .Where(c => c.Estado == ComisionEstados.DestinosDefinidos)
        .Select(c => new
        {
            c.ComisionId,
            c.Precio_Gasolina_Usado,
            c.Presupuesto_Combustible_Estimado,

            FechaSalida = c.Nombramiento_Respon_Vehiculo!.Fecha_Salida,
            FechaRegreso = c.Nombramiento_Respon_Vehiculo.Fecha_Regreso,

            Departamentos = c.Nombramiento_Respon_Vehiculo.NomMunicipios!
                .Select(nm => nm.Municipio!.Departamento.Nombre),

            Municipios = c.Nombramiento_Respon_Vehiculo.NomMunicipios!
                .Select(nm => nm.Municipio!.Nombre),

            Destinos = c.ComisionDestinos!
                .Select(cd => new
                {
                    cd.Descripcion,
                    cd.Kilometros
                }),

            Kilometros = c.ComisionDestinos!
                .Sum(x => x.Kilometros)
        })
        .ToListAsync();
        
        return comisiones.Select(c => new GetComisionesPendApprovResponse
        {
            ComisionId = c.ComisionId,

            Departamento = string.Join(
                ", ",
                c.Departamentos.Distinct()
            ),

            Municipio = string.Join(
                ", ",
                c.Municipios.Distinct()
            ),

            Destinos = c.Destinos
                .Select(x => $"{x.Descripcion} - {x.Kilometros}")
                .ToList(),

            Fecha_Salida = c.FechaSalida,

            Fecha_Regreso = c.FechaRegreso,

            Kilometros = c.Kilometros,

            Precio_Gasolina = c.Precio_Gasolina_Usado,

            Prespuesto_Estimado = c.Presupuesto_Combustible_Estimado

        }).ToList();
    }

    public async Task<PlanViajeResponse?> GetPlanViajeResponseAsync(int idUsuario, int idComision)
    {
        var query = 
            from n in _context.Nombramientos
            join c in _context.Comisiones
                on n.ComisionId equals c.ComisionId
            join u in _context.Users
                on n.AsignacionUsuario!.UsuarioId equals u.Id
            where c.ComisionId == idComision
            && n.AsignacionUsuario!.UsuarioId == idUsuario
            select new PlanViajeResponse
            {
                Fecha_Salida = n.Fecha_Salida,
                Fecha_Regreso = n.Fecha_Regreso,
                Descripcion = n.Descripcion,
                TotalCombustibleAutorizado = c.Presupuesto_Combustible_Aprobado,
                Es_Gasolina = (CombustibleTipos)c.Vehiculo!.Combustible!.CombustibleId != CombustibleTipos.Disel ? true : false,
                Precio_Galon = c.Precio_Gasolina_Usado,
                Viaticos = n.ComisionViaticosList!.Select(cv=> new GetComisionViaticoResponse
                {
                    Tipo_viatico = cv.Viatico!.Nombre,
                    Monto = cv.Viatico.Monto,
                    Fecha = DateOnly.FromDateTime(cv.Fecha)
                }).ToList(),
                Destinos = c.ComisionDestinos!.Select(cd => new GetComisionDestinosDetailResponse
                {
                    Descripcion = cd.Descripcion,
                    Kilometros = cd.Kilometros,
                    Galones = cd.Galones
                }).ToList(),
                Nombre = $"{u.Nombres} {u.Apellidos}"
            };
        return await query.FirstOrDefaultAsync();                    
    }
    public async Task<Result<PagedList<GetComisionesDetalleResponse>>> GetComisionesDetalleAsync(GetComisionesDetalleRequest request)
    {
        IQueryable<Comision> queryable = _context.Comisiones.AsNoTracking();
        
        var predicate = ExpressionBuilder.New<Comision>();

        // if(request.Fecha_Inicio is not null && request.Fecha_Fin is not null)
        // {
        //     predicate = predicate.And(x=>
        //                     x.Fecha_Salida <= request.Fecha_Fin && x.Fecha_Regreso>=request.Fecha_Inicio 
        //                 );
        // }
        // if(!string.IsNullOrEmpty(request.Departamento))
        // {
        //     predicate = predicate.And(x=>
        //                     x.Departamento!
        //                     .Contains(request.Departamento)
        //                 );
        // }
        if(!string.IsNullOrEmpty(request.Estado))
        {
            predicate = predicate.And(x=>
                            x.Estado!
                            .Contains(request.Estado)
                        );
        }
        // if(!string.IsNullOrEmpty(request.OrderBy))
        // {
        //     Expression<Func<Comision, object>> orderBySelector =
        //                 request.OrderBy.ToLower() switch
        //                 {
        //                     "fecha_inicio" => com => com.Fecha_Salida,
        //                     "fecha_fin" => com => com.Fecha_Regreso,
        //                     "departamento" => com => com.Departamento!,
        //                     "estado" => com => com.Estado!,
        //                     _ => com => com.ComisionId
        //                 };
        //     bool orderBy = request.OrderAsc.HasValue
        //                     ? request.OrderAsc.Value
        //                     : true;
        //     queryable = orderBy ? queryable.OrderBy(orderBySelector) : queryable.OrderByDescending(orderBySelector);
        // }
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