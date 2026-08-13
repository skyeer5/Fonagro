using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using WebApp.Application.Core;
using WebApp.Application.Interfaces;
using WebApp.Application.Usuarios.Queries.GetUsuariosActivosDetalle;
using WebApp.Domain.AsignacionUsuarios;
using WebApp.Domain.Usuarios;
using WebApp.Persistence;
using static WebApp.Application.Usuarios.Queries.GetUsuariosActivosDetalle.GetUsuariosActivosDetalleQuery;

namespace WebApp.Infrastructure.Services;

public class AsignacionUsuarioService : IAsignacionUsuarioService
{
    private readonly WebAppDbContext _context;
    public AsignacionUsuarioService(WebAppDbContext context)
    {
        _context = context;
    }

    public async Task<int?> GetAsignacionUsuarioIdByUsuarioIdAsync(int usuarioId, CancellationToken cancellationToken)
    {
        var asignacionUsuario = await _context.AsignacionesUsuarios!
            .Where(au => au.UsuarioId == usuarioId && au.Fecha_Desasignacion == null)
            .FirstOrDefaultAsync(cancellationToken);

        return asignacionUsuario?.AsignacionUsuarioId;
    }
    
    public async Task<Result<PagedList<GetUsuariosActivosDetalleResponse>>> GetUsuariosActivosDetalleAsync(GetUsuariosActivosDetalleQueryRequest request)
    {
        var usersQuery = from au in _context.AsignacionesUsuarios
                        join u in _context.Users
                            on au.UsuarioId equals u.Id
                        where au.Fecha_Desasignacion == null
                        select new GetUsuariosActivosDetalleResponse
                        {
                            Id = u.Id,
                            Nombre_Completo = $"{u.Nombres} {u.Apellidos}",
                            NIT = u.NIT,
                            Puesto = au.Puesto!.Nombre,
                            Unidad = au.Puesto.Unidad!.Nombre,
                            Estado = u.Estado
                        };
        var pagination = await PagedList<GetUsuariosActivosDetalleResponse>.CreateAsync(
                                        usersQuery,
                                        request.request.PageNumber,
                                        request.request.PageSize
            
        );
        return Result<PagedList<GetUsuariosActivosDetalleResponse>>.Success(pagination);
    }
}