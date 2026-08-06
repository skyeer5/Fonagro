using Microsoft.EntityFrameworkCore;
using WebApp.Application.Core;
using WebApp.Application.Interfaces;
using WebApp.Application.Nombramientos.Queries.GetNomParaAprobar;
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

    public async Task<PagedList<GetNomParaAprobarResponse>> GetListNMParaAprobarAsync(GetNomParaAprobarRequest request,CancellationToken cancellationToken)
    {
        var query =
            from n in _context.Nombramientos

            join usuarioNombrado in _context.Users
                on n.AsignacionUsuario!.UsuarioId equals usuarioNombrado.Id

            join usuarioCreador in _context.Users
                on n.UsuarioId_Creador equals usuarioCreador.Id

            select new GetNomParaAprobarResponse
            {
                NombramientoId = n.NombramientoId,

                Correlativo = $"FON-{((UnidadesEnum)n.AsignacionUsuario!.Puesto!.UnidadId).ToString()}-{n.Correlativo}-{DateTime.Now.Year}",

                Nombre_Nombrado =
                    usuarioNombrado.Nombres + " " +
                    usuarioNombrado.Apellidos,

                Nombre_Creador_Nombramiento =
                    usuarioCreador.Nombres + " " +
                    usuarioCreador.Apellidos,

                Fecha_Salida = n.Fecha_Salida,

                Fecha_Regreso = n.Fecha_Regreso,

                Proposito = n.Proposito
            };
        var pagination = await PagedList<GetNomParaAprobarResponse>.CreateAsync(
                                    query,
                                    request.PageNumber,
                                    request.PageSize
        );
        return pagination;
    }
}