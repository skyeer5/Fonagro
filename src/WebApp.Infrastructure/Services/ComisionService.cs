using System.Linq.Expressions;
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
using WebApp.Domain.Gasolinas;
using WebApp.Persistence;
using WebApp.Domain.Comisiones;
using WebApp.Domain.Unidades;
using WebApp.Application.Comisiones.Queries.GetComisionesExcel;

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
                        Fecha_Salida = n.Fecha_Salida.ToDateTime(c.Hora_Salida),
                        Fecha_Regreso = n.Fecha_Regreso.ToDateTime(c.Hora_Regreso),
                        Estado = c.Estado,
                        Descripcion = n.Descripcion,
                        Prespuesto_Aprobado = c.Presupuesto_Combustible_Aprobado.HasValue,
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
        return await query.AsSplitQuery().FirstOrDefaultAsync();                

    }

    public async Task<Comision?> GetComisionByIdAsync(int comisionId)
    {
       return await _context.Comisiones.Where(x=>x.ComisionId == comisionId).Include(x=>x.Vehiculo).FirstOrDefaultAsync();
    }

    public async Task<PagedList<GetComisionesPendApprovResponse>> GetComisionPendApprovAsync(GetComisionesPendApprovRequest request)
    {
        var queryable = _context.Comisiones.AsNoTracking().AsSingleQuery();
        
        if(request.Estado != 0)
            queryable = request.Estado switch
            {
                1 => queryable.Where(x=>x.Estado == ComisionEstados.DestinosDefinidos),
                2 => queryable.Where(x=> x.Estado != ComisionEstados.Creada && x.Estado != ComisionEstados.DestinosDefinidos && x.UsuarioId_Aprobador_Combustible != null),
                3 => queryable.Where(x=> (x.Estado == ComisionEstados.DestinosDefinidos) || (x.Estado != ComisionEstados.Creada && x.Estado != ComisionEstados.DestinosDefinidos && x.UsuarioId_Aprobador_Combustible != null)),
                _ => queryable.Where(x=>x.Estado == ComisionEstados.DestinosDefinidos)
            };
        if(request.Fecha_Inicio is not null)
            queryable = queryable.Where(x=>x.Nombramiento_Respon_Vehiculo!.Fecha_Salida>= request.Fecha_Inicio);

        if(request.Fecha_Fin is not null)
            queryable = queryable.Where(x=>x.Nombramiento_Respon_Vehiculo!.Fecha_Regreso<= request.Fecha_Fin);

        var comisionsQuery = queryable.Select(c => new GetComisionesPendApprovResponse
        {
            ComisionId = c.ComisionId,

            DepartamentosYMunicipios = c.Nombramiento_Respon_Vehiculo!.NomMunicipios!
                .Select(x=> $"{x.Municipio!.Departamento.Nombre} - {x.Municipio.Nombre}")
                .ToList(),

            Destinos = c.ComisionDestinos!
                .Select(x => $"{x.Descripcion} - {x.Kilometros}")
                .ToList(),

            Fecha_Salida = c.Nombramiento_Respon_Vehiculo!.Fecha_Salida,

            Fecha_Regreso = c.Nombramiento_Respon_Vehiculo!.Fecha_Regreso,

            Kilometros = c.ComisionDestinos!.Sum(x=>x.Kilometros),

            Precio_Gasolina = c.Precio_Gasolina_Usado,

            Prespuesto_Estimado = c.Presupuesto_Combustible_Estimado,

            GalonesEstimados = c.ComisionDestinos!.Sum(x=>x.Galones),

            ComsumoKmPorGalon = c.Vehiculo!.ConsumoKmPorGalon,

            PresupuestoAprobado = c.Presupuesto_Combustible_Aprobado

        });
        return await PagedList<GetComisionesPendApprovResponse>.CreateAsync(
                                    comisionsQuery,
                                    request.PageNumber,
                                    request.PageSize
        );
        
    }

    public async Task<PlanViajeDto?> GetPlanViajeResponseAsync(int idUsuario, int idComision)
    {
        var query = 
            from n in _context.Nombramientos
            join c in _context.Comisiones
                on n.ComisionId equals c.ComisionId
            join u in _context.Users
                on n.AsignacionUsuario!.UsuarioId equals u.Id
            where c.ComisionId == idComision
            && n.AsignacionUsuario!.UsuarioId == idUsuario
            select new PlanViajeDto
            {
                Fecha_Salida = n.Fecha_Salida.ToDateTime(c.Hora_Salida),
                Fecha_Regreso = n.Fecha_Regreso.ToDateTime(c.Hora_Regreso),
                Descripcion = n.Descripcion,
                TotalCombustibleAutorizado = c.Presupuesto_Combustible_Aprobado.HasValue ? c.Presupuesto_Combustible_Aprobado.Value : 0,
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
        return await query.AsSplitQuery().FirstOrDefaultAsync();                    
    }
    public async Task<Result<PagedList<GetComisionesDetalleResponse>>> GetComisionesDetalleAsync(GetComisionesDetalleRequest request)
    {
        var queryable = _context.Comisiones.AsNoTracking().AsQueryable();
        
        if(request.Fecha_Inicio is not null && request.Fecha_Fin is not null)
            queryable = queryable.Where(x=> 
            x.Nombramiento_Respon_Vehiculo!.Fecha_Regreso <= request.Fecha_Fin 
            && x.Nombramiento_Respon_Vehiculo.Fecha_Salida>=request.Fecha_Inicio);

        if (request.Departamentos != null && request.Departamentos.Count != 0)
            queryable = queryable.Where(x => x.Nombramiento_Respon_Vehiculo!.NomMunicipios!.Any(nm => request.Departamentos.Contains(nm.Municipio!.DepartamentoId)));

        if (request.Municipios != null && request.Municipios.Count != 0)
            queryable = queryable.Where(x => x.Nombramiento_Respon_Vehiculo!.NomMunicipios!.Any(nm => request.Municipios.Contains(nm.MunicipioId)));

        if (request.Estado.HasValue)
            queryable = queryable.Where(x => x.Estado == (ComisionEstados)request.Estado);
                bool orderBy = request.OrderAsc ?? false;
        
        Expression<Func<Comision, object>> orderBySelector =
                    request.OrderBy?.ToLower() switch
                    {
                        "fecha_inicio" => v => v.Nombramiento_Respon_Vehiculo!.Fecha_Salida!,
                        "fecha_fin" => v => v.Nombramiento_Respon_Vehiculo!.Fecha_Regreso!,
                        "estado" => v => v.Estado!,
                        _ => v => v.ComisionId
                    };
        
        queryable = orderBy ? queryable.OrderBy(orderBySelector) : queryable.OrderByDescending(orderBySelector);


        var comisionsQuery = queryable.Select(x=> new GetComisionesDetalleResponse
        {
            ComisionId = x.ComisionId,
            DepartamentosYMunicipios = x.Nombramiento_Respon_Vehiculo!.NomMunicipios!.Select(nms => 
                $"{nms.Municipio!.Departamento.Nombre} - {nms.Municipio.Nombre}").ToList(),
            Descripcion_Vehiculo = $"{x.Vehiculo!.Placa} / {x.Vehiculo.Marca} {x.Vehiculo.Modelo}",
            Estado = x.Estado.ToString(),
            Fecha_Salida = x.Nombramiento_Respon_Vehiculo.Fecha_Salida.ToDateTime(x.Hora_Salida),
            Fecha_Regreso = x.Nombramiento_Respon_Vehiculo.Fecha_Regreso.ToDateTime(x.Hora_Regreso)
        });
        var pagination = await PagedList<GetComisionesDetalleResponse>.CreateAsync(
                                    comisionsQuery,
                                    request.PageNumber,
                                    request.PageSize
        );
        return Result<PagedList<GetComisionesDetalleResponse>>.Success(pagination);
    }

    public async Task<List<GetComisionesExcelDto>?> GetComisionesExcelDtos(GetComisionesExcelRequest request, CancellationToken cancellationToken)
    {
        var query = _context.Comisiones.AsNoTracking().AsQueryable();

        if (request.Fecha_Salida.HasValue)
            query = query.Where(x => x.Nombramiento_Respon_Vehiculo!.Fecha_Salida >= request.Fecha_Salida);

        if (request.Fecha_Regreso.HasValue)
            query = query.Where(x => x.Nombramiento_Respon_Vehiculo!.Fecha_Regreso <= request.Fecha_Regreso);

        if (request.Departamentos != null && request.Departamentos.Count != 0)
            query = query.Where(x => x.Nombramiento_Respon_Vehiculo!.NomMunicipios!.Any(nm => request.Departamentos.Contains(nm.Municipio!.DepartamentoId)));

        if (request.Municipios != null && request.Municipios.Count != 0)
            query = query.Where(x => x.Nombramiento_Respon_Vehiculo!.NomMunicipios!.Any(nm => request.Municipios.Contains(nm.MunicipioId)));

        if (request.Vehiculo.HasValue)
            query = query.Where(x => x.VehiculoId == request.Vehiculo);

        if (request.Usuario.HasValue)
            query = query.Where(x => x.Nombramientos!.Any(n => n.AsignacionUsuario!.UsuarioId == request.Usuario));

        if (request.Unidades != null && request.Unidades.Count != 0)
            query = query.Where(x => x.Nombramientos!.Any(n => request.Unidades.Contains(n.AsignacionUsuario!.Puesto!.UnidadId)));

        if (request.Estado.HasValue)
            query = query.Where(x => x.Estado == (ComisionEstados)request.Estado);

        var rawComisiones = await query.Select(c => new
        {
            ComisionId = c.ComisionId,
            Fecha_Salida = c.Nombramiento_Respon_Vehiculo!.Fecha_Salida,
            Fecha_Regreso = c.Nombramiento_Respon_Vehiculo.Fecha_Regreso,
            Hora_Salida = c.Hora_Salida,
            Hora_Regreso = c.Hora_Regreso,
            Fecha_Creacion_Comision = c.Fecha,
            DepartamentosYMunicipios = c.Nombramiento_Respon_Vehiculo.NomMunicipios!.Select(nms => new GetComisionesExcelDestinosDto
            {
                Departamento = nms.Municipio!.Departamento.Nombre,
                Municipio = nms.Municipio.Nombre
            }).ToList(),
            Destinos = c.ComisionDestinos!.Select(x => $"{x.Descripcion} - {x.Kilometros}").ToList(),
            PlacaVehiculo = c.Vehiculo != null ? c.Vehiculo.Placa : "",
            ModeloVehiculo = c.Vehiculo != null ? c.Vehiculo.Modelo : "",
            
            UsuarioResponsableId = c.Nombramiento_Respon_Vehiculo.AsignacionUsuario!.UsuarioId,
            UsuariosNombradosIds = c.Nombramientos!.Select(n => n.AsignacionUsuario!.UsuarioId).ToList(),
            Estado = c.Estado.ToString(),
            PresupuestoCombustibleEstimado = c.Presupuesto_Combustible_Estimado,
            PresupuestoCombustibleAprobado = c.Presupuesto_Combustible_Aprobado.HasValue ? c.Presupuesto_Combustible_Aprobado.Value : 0,
            PrecioCombustible = c.Precio_Gasolina_Usado
        }).AsSplitQuery().ToListAsync(cancellationToken);

        if (!rawComisiones.Any()) return new List<GetComisionesExcelDto>();

        // 3. Recolectamos TODOS los UsuarioIds únicos para hacer UNA SOLA consulta a Users
        var userIds = rawComisiones
            .Select(c => c.UsuarioResponsableId)
            .Concat(rawComisiones.SelectMany(c => c.UsuariosNombradosIds))
            .Distinct()
            .ToList();

        // 4. Consultamos la tabla de usuarios UNA SOLA VEZ y creamos un diccionario en memoria
        var usuariosDic = await _context.Users
            .AsNoTracking()
            .Where(u => userIds.Contains(u.Id))
            .Select(u => new { u.Id, NombreCompleto = $"{u.Nombres} {u.Apellidos}" })
            .ToDictionaryAsync(u => u.Id, u => u.NombreCompleto, cancellationToken);

        // 5. Mapeo final ensamblando los nombres desde el Diccionario (búsquedas O(1) ultra rápidas)
        var resultado = rawComisiones.Select(c => new GetComisionesExcelDto
        {
            ComisionId = c.ComisionId,
            Fecha_Salida = c.Fecha_Salida.ToDateTime(c.Hora_Salida),
            Fecha_Regreso = c.Fecha_Regreso.ToDateTime(c.Hora_Regreso),
            Fecha_Creacion_Comision = c.Fecha_Creacion_Comision,
            DepartamentosYMunicipios = c.DepartamentosYMunicipios,
            Destinos = c.Destinos,
            Vehiculo = string.IsNullOrEmpty(c.PlacaVehiculo) ? "Sin Vehículo" : $"{c.PlacaVehiculo} - {c.ModeloVehiculo}",
            
            // Obtener el nombre del responsable desde el diccionario
            NombreResponsableVehiculo = usuariosDic.TryGetValue(c.UsuarioResponsableId, out var resp) ? resp : "Desconocido",
            NombreCreadorComision = usuariosDic.TryGetValue(c.UsuarioResponsableId, out var creador) ? creador : "Desconocido",
            
            // Mapear la lista de nombrados
            Nombrados = c.UsuariosNombradosIds
                .Where(id => usuariosDic.ContainsKey(id))
                .Select(id => usuariosDic[id])
                .ToList(),
            Estado = c.Estado.ToString(),
            PresupuestoCombustibleEstimado = c.PresupuestoCombustibleEstimado,
            PresupuestoCombustibleAprobado = c.PresupuestoCombustibleAprobado,
            PrecioCombustible = c.PrecioCombustible
        }).ToList();

        return resultado;
    }

    public async Task<Comision?> GetComisionToCancelAsync(int comisionId, CancellationToken cancellationToken)
    {
        return await _context.Comisiones
                                .Where(x=>x.ComisionId == comisionId)
                                .Include(x=>x.Vehiculo)
                                .Include(x=>x.ComisionDestinos)
                                .Include(x=>x.Nombramientos!)
                                    .ThenInclude(cu=>cu.ComisionViaticosList)
                                .AsSplitQuery()
                                .FirstOrDefaultAsync(cancellationToken);
    }
}