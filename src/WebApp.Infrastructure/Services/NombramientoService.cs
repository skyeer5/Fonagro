using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using WebApp.Application.Core;
using WebApp.Application.Interfaces;
using WebApp.Application.Nombramientos.Queries.GetNombramientoById;
using WebApp.Application.Nombramientos.Queries.GetNombramientos;
using WebApp.Application.Nombramientos.Queries.GetNomDatosById;
using WebApp.Application.Nombramientos.Queries.GetNomsApproved;
using WebApp.Application.Nombramientos.Queries.NombramientoPdf;
using WebApp.Domain.Nombramientos;
using WebApp.Domain.Unidades;
using WebApp.Persistence;

namespace WebApp.Infrastructure.Services;

public class NombramientoService : INombramientoService
{
    private readonly WebAppDbContext _context;
    private readonly ICurrentUser _currentUser;
    private readonly IAsignacionUsuarioService _asignacionUsuarioService;

    public NombramientoService(WebAppDbContext context, ICurrentUser currentUser, IAsignacionUsuarioService asignacionUsuarioService)
    {
        _context = context;
        _currentUser = currentUser;
        _asignacionUsuarioService = asignacionUsuarioService;
    }

    public async Task<Nombramiento?> GetNMByIdComisionAndUsuarioIdAsync(int comisionId, CancellationToken cancellationToken)
    {
        var userId = _currentUser.userId;
        var usuarioPuestoId = await _asignacionUsuarioService.GetAsignacionUsuarioIdByUsuarioIdAsync(userId, cancellationToken);
        return await _context.Nombramientos!.Where(cu=>cu.ComisionId == comisionId && cu.AsignacionUsuarioId == usuarioPuestoId)
                                        .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<string?> GetDescripcionAsync(int comisionId, int usuarioId, CancellationToken cancellationToken)
    {
        var usuarioPuestoId = await _asignacionUsuarioService.GetAsignacionUsuarioIdByUsuarioIdAsync(usuarioId, cancellationToken);

        return await _context.Nombramientos!.Where(cu=>cu.ComisionId == comisionId && cu.AsignacionUsuarioId == usuarioPuestoId)
                                        .Select(cu => cu.Descripcion)
                                        .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<PagedList<GetNombramientosResponse>> GetNombramientosAsync(GetNombramientosRequest request,CancellationToken cancellationToken)
    {

        var query = 
            from n in _context.Nombramientos
            join usuarioNombrado in _context.Users
                on n.AsignacionUsuario!.UsuarioId equals usuarioNombrado.Id
            select new 
            {
                n.NombramientoId,
                UnidadId = n.AsignacionUsuario!.Puesto!.UnidadId,
                n.Correlativo,
                Anio = n.Fecha_Creado.Year,
                Nombre_Nombrado = usuarioNombrado.Nombres + " " + usuarioNombrado.Apellidos,
                n.Fecha_Salida,
                n.Fecha_Regreso,
                Municipios = n.NomMunicipios!.Select(x=> $"{x.Municipio!.Departamento.Nombre} - {x.Municipio.Nombre}"),
                n.Estado
            };

        if(request.Correlativo.HasValue) 
            query = query.Where(x=>x.Correlativo == request.Correlativo);

        if(request.Unidad.HasValue) 
            query = query.Where(x=> x.UnidadId == request.Unidad);
        
        if(request.Fecha_Inicio.HasValue)
            query = query.Where(x=> x.Fecha_Salida >= request.Fecha_Inicio);
        
        if(request.Fecha_Fin.HasValue)
            query = query.Where(x=>x.Fecha_Regreso <= request.Fecha_Fin);
        
        if(!request.Nombre_Nombrado.IsNullOrEmpty())
            query = query.Where(x=>x.Nombre_Nombrado.Contains(request.Nombre_Nombrado!));
        if(request.Estado.HasValue)
            query = query.Where(x=> x.Estado == (NombramientoEstados)request.Estado);

        var pagedListAnonimo = await PagedList<dynamic>.CreateAsync(
            query,
            request.PageNumber,
            request.PageSize
        );

        var responseItems = pagedListAnonimo.Items.Select(x => new GetNombramientosResponse
        {
            NombramientoId = x.NombramientoId,
            Correlativo = $"FON-{(UnidadesEnum)x.UnidadId}-{x.Correlativo}-{x.Anio}",
            Nombre_Nombrado = x.Nombre_Nombrado,
            Fecha_Salida = x.Fecha_Salida,
            Fecha_Regreso = x.Fecha_Regreso,
            DepartamentosYMunicipios = x.Municipios,
            Estado = x.Estado
        }).ToList();

        return new PagedList<GetNombramientosResponse>(
            responseItems,
            pagedListAnonimo.TotalCount,
            pagedListAnonimo.CurrentPage,
            pagedListAnonimo.PageSize
        );
    }

    public async Task<List<GetNomsApprovedResponse>> GetNomsApprovedAsync(CancellationToken cancellationToken)
    {
        var query = from n in _context.Nombramientos
                    join u in _context.Users
                        on n.AsignacionUsuario!.UsuarioId equals u.Id
                    where n.Estado == NombramientoEstados.Aprobado && n.Comision == null
                    select new GetNomsApprovedResponse
                    {
                        NombramientoId = n.NombramientoId,
                        Descripcion = $"{u.Nombres} {u.Apellidos} | FON-{((UnidadesEnum)n.AsignacionUsuario!.Puesto!.UnidadId).ToString()}-{n.Correlativo}-{n.Fecha_Creado.Year}"
                    };
        return await query.ToListAsync(cancellationToken);
    }

    public async Task<GetNomDatosByIdResponse?> GetNomDatosByIdAsync(int nombramientoId, CancellationToken cancellationToken)
    {
        return await _context.Nombramientos
            .Where(x => x.NombramientoId == nombramientoId)
            .Select(x => new GetNomDatosByIdResponse
            {
                Departamentos = string.Join(", ", x.NomMunicipios!
                    .Select(nm => nm.Municipio!.Departamento.Nombre)
                    .Distinct()), 
                    
                Municipios = string.Join(", ", x.NomMunicipios!
                    .Select(nm => nm.Municipio!.Nombre)),
                    
                Fecha_Regreso = x.Fecha_Regreso,
                Fecha_Salida = x.Fecha_Salida
            })
            .AsSplitQuery()
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<List<Nombramiento>?> GetNombramientosByListIdsAsync(List<int> nombramientos, CancellationToken cancellationToken)
    {
        return await _context.Nombramientos
                        .Where(x=>nombramientos.Contains(x.NombramientoId))
                        .Include(x=>x.NomMunicipios!)
                            .ThenInclude(nm=>nm.Municipio)
                        .ToListAsync();
    }

    public async Task<NombramientoPdfDto?> GetNombramientoPdfDtoAsync(int nombramientoId, CancellationToken cancellationToken)
    {
        var query = from n in _context.Nombramientos 
                    join au in _context.AsignacionesUsuarios
                            on n.AsignacionUsuarioId equals au.AsignacionUsuarioId
                    join u in _context.Users 
                            on au.UsuarioId equals u.Id
                    join uc in _context.Users
                            on n.UsuarioId_Creador equals uc.Id
                    where n.NombramientoId == nombramientoId
                    select new NombramientoPdfDto
                    {
                        NumeroNombramiento = $"FON-{((UnidadesEnum)au.Puesto!.UnidadId).ToString()}-{n.Correlativo}-{n.Fecha_Creado.Year}",
                        FechaCreacion = n.Fecha_Creado,
                        NombreCompleto = $"{u.Nombres} {u.Apellidos}",
                        Puesto = au.Puesto.Nombre!,
                        Proposito = n.Proposito!,
                        Destinos = n.NomMunicipios!.Select(nms=>
                                        new NombramientoPdfDestinosDto
                                        {
                                            Departamento = nms.Municipio!.Departamento.Nombre,
                                            Municipio = nms.Municipio.Nombre
                                        }
                                    ).ToList(),
                        FechaInicio = n.Fecha_Salida,
                        FechaFin = n.Fecha_Regreso,
                        EmitidoPor = $"{uc.Nombres} {uc.Apellidos}"
                    };
        return await query.FirstOrDefaultAsync();
    }

    public async Task<GetNombramientoByIdResponse?> GetNombramientoByIdResponseAsync(int nombramientoId, CancellationToken cancellationToken)
    {
        var query = from n in _context.Nombramientos
                    join u in _context.Users
                        on n.AsignacionUsuario!.UsuarioId equals u.Id
                    where n.NombramientoId == nombramientoId
                    select new GetNombramientoByIdResponse
                    {
                        NombramientoId = n.NombramientoId,
                        Nombre_Completo = $"{u.Nombres} {u.Apellidos}",
                        Puesto = n.AsignacionUsuario!.Puesto!.Nombre!,
                        Unidad = n.AsignacionUsuario.Puesto.Unidad!.Nombre!,
                        Correlativo = $"FON-{((UnidadesEnum)n.AsignacionUsuario.Puesto!.UnidadId).ToString()}-{n.Correlativo}-{n.Fecha_Creado.Year}",
                        Proposito = n.Proposito!,
                        NombramientoEstado = n.Estado,
                        Municipios = n.NomMunicipios!.Select(x=> x.MunicipioId).ToList(),
                        Departamentos = n.NomMunicipios!.Select(x=>x.Municipio!.DepartamentoId).ToList(),
                        Fecha_Salida = n.Fecha_Salida,
                        Fecha_Regreso = n.Fecha_Regreso,
                        ComisionId = n.ComisionId != null ? n.ComisionId : null,
                        ComisionEstado = n.Comision != null ? n.Comision.Estado : null
                    };
                    
        return await query.AsSplitQuery().FirstOrDefaultAsync(cancellationToken);
    }
}