using Microsoft.EntityFrameworkCore;
using WebApp.Application.Core;
using WebApp.Application.Interfaces;
using WebApp.Application.Nombramientos.Queries.GetNombramientos;
using WebApp.Application.Nombramientos.Queries.GetNomDatosById;
using WebApp.Application.Nombramientos.Queries.GetNomsApproved;
using WebApp.Domain.Nombramientos;
using WebApp.Domain.NomMunicipios;
using WebApp.Domain.Unidades;
using WebApp.Persistence;
using WebApp.Persistence.Models;

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

            join usuarioCreador in _context.Users
                on n.UsuarioId_Creador equals usuarioCreador.Id

            select new GetNombramientosResponse
            {
                NombramientoId = n.NombramientoId,

                Correlativo = $"FON-{((UnidadesEnum)n.AsignacionUsuario!.Puesto!.UnidadId).ToString()}-{n.Correlativo}-{n.Fecha_Creado.Year}",

                Nombre_Nombrado =
                    usuarioNombrado.Nombres + " " +
                    usuarioNombrado.Apellidos,

                Nombre_Creador_Nombramiento =
                    usuarioCreador.Nombres + " " +
                    usuarioCreador.Apellidos,

                Fecha_Salida = n.Fecha_Salida,

                Fecha_Regreso = n.Fecha_Regreso,

                Proposito = n.Proposito,

                Estado = n.Estado
            };
        var pagination = await PagedList<GetNombramientosResponse>.CreateAsync(
                                    query,
                                    request.PageNumber,
                                    request.PageSize
        );
        return pagination;
    }

    public async Task<List<GetNomsApprovedResponse>> GetNomsApprovedAsync(CancellationToken cancellationToken)
    {
        var query = from n in _context.Nombramientos
                    join u in _context.Users
                        on n.AsignacionUsuario!.UsuarioId equals u.Id
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
            .FirstOrDefaultAsync(cancellationToken);
    }
}