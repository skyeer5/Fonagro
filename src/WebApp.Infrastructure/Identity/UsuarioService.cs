using System.Linq.Expressions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WebApp.Application.Core;
using WebApp.Application.Interfaces;
using WebApp.Application.Usuarios.Queries.GetUsuarios;
using WebApp.Application.Usuarios.Queries.GetUsuariosActivos;
using WebApp.Application.Usuarios.Queries.GetUsuariosActivosDetalle;
using WebApp.Application.Usuarios.Queries.GetUsuariosSinComision;
using WebApp.Application.Usuarios.Queries.GetUsuariosSinNom;
using WebApp.Domain.Usuarios;
using WebApp.Persistence.Models;

namespace WebApp.Infrastructure.Identity;

public class UsuarioService : IUsuarioService
{
    private readonly UserManager<AppUser> _userManager;

    public UsuarioService(UserManager<AppUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task<string?> GetNombreUsuarioAsync(int usuarioId)
    {
        return await _userManager.Users.Where(u => u.Id == usuarioId)
            .Select(u => u.Nombres)
            .FirstOrDefaultAsync();
    }

    public async Task<List<GetUsuariosActivosResponse>> GetUsuariosActivosAsync()
    {
        return await _userManager.Users
            .Where(u => u.Estado == UsuarioEstados.Activo)
            .Select(u => new GetUsuariosActivosResponse
            {
                Id = u.Id,
                Nombre_Completo = u.Nombres
            })
            .ToListAsync();
    }

    public async Task<Result<PagedList<GetUsuariosActivosDetalleResponse>>> GetUsuariosActivosDetalleAsync(GetUsuariosActivosDetalleRequest request)
    {
        var queryable = _userManager.Users.AsNoTracking();

        queryable = (UsuarioEstados)request.Estado!.Value switch
        {
            UsuarioEstados.Activo => queryable.Where(x => x.UsuarioPuestos!.Any(up => up.Fecha_Desasignacion == null)),
            UsuarioEstados.Baja   => queryable.Where(x => !x.UsuarioPuestos!.Any(up => up.Fecha_Desasignacion == null)),
            _ => queryable.Where(x => x.UsuarioPuestos!.Any(up => up.Fecha_Desasignacion == null))
        };

        if (!string.IsNullOrWhiteSpace(request.Nombre))
        {
            var nombreFiltro = request.Nombre.Trim();
            queryable = queryable.Where(x => (x.Nombres + " " + x.Apellidos).Contains(nombreFiltro));
        }

        bool isAscending = request.OrderAsc ?? true;

        Expression<Func<AppUser, object>> orderBySelector = (request.OrderBy?.ToLower()) switch
        {
            "nombre" => user => user.Nombres!,
            _        => user => user.Id!
        };

        queryable = isAscending 
            ? queryable.OrderBy(orderBySelector) 
            : queryable.OrderByDescending(orderBySelector);

        var usersQuery = queryable.Select(x => new GetUsuariosActivosDetalleResponse
        {
            Id = x.Id,
            Nombre_Completo = $"{x.Nombres} {x.Apellidos}",
            Puesto = x.UsuarioPuestos!
                .Where(up => up.Fecha_Desasignacion == null)
                .Select(up => up.Puesto!.Nombre)
                .FirstOrDefault(),
            Estado = x.UsuarioPuestos!.Any(up => up.Fecha_Desasignacion == null) 
                ? UsuarioEstados.Activo 
                : UsuarioEstados.Baja,
            Unidad = x.UsuarioPuestos!
                .Where(up => up.Fecha_Desasignacion == null)
                .Select(up => up.Puesto!.Unidad!.Nombre)
                .FirstOrDefault(),
            NIT = x.NIT
        });

        var pagination = await PagedList<GetUsuariosActivosDetalleResponse>.CreateAsync(
            usersQuery,
            request.PageNumber,
            request.PageSize
        );

        return Result<PagedList<GetUsuariosActivosDetalleResponse>>.Success(pagination);
    }

    public async Task<List<GetUsuariosResponse>?> GetUsuariosAsync(CancellationToken cancellationToken)
    {
        return await _userManager.Users.AsNoTracking().Select(x=> new GetUsuariosResponse
        {
            UsuarioId = x.Id,
            Nombre_Completo = $"{x.Nombres} {x.Apellidos}",
        }).ToListAsync(cancellationToken);
    }

    public async Task<List<GetUsuariosSinComisionResponse>> GetUsuariosSinComisionAsync()
    { 
        return await _userManager.Users
            .Where(u => u.Estado == UsuarioEstados.Activo /*&& !u.UsuarioPuestos!.Any(up => up.Nombramientos!.Any(cu => cu.Comision!.Estado != ComisionEstados.Completada && cu.Comision.Estado != ComisionEstados.Cancelada))*/)
            .OrderBy(x=> x.Nombres)
            .Select(u => new GetUsuariosSinComisionResponse
            {
                Id = u.Id,
                Nombre_Completo = u.Nombres
            })
            .ToListAsync();
    }

    public Task<List<GetUsuariosSinNomResponse>> GetUsuariosSinNomAsync()
    {
        return _userManager.Users
            .SelectMany(u => u.UsuarioPuestos!
                .Where(a => a.Fecha_Desasignacion == null)
                .Select(a => new GetUsuariosSinNomResponse
                {
                    UsuarioId = a.AsignacionUsuarioId,
                    Nombre_Completo = u.Nombres + " " + u.Apellidos
                })
            ).ToListAsync();
    }

    public async Task<bool> UsuarioExistsAsync(int usuarioId, CancellationToken cancellationToken)
    {
        return await _userManager.Users.AnyAsync(x=>x.Id == usuarioId, cancellationToken);
    }

    public async Task<bool> UsuariosExistsAsync(int usuarioId, List<int> usuariosIds)
    {
        var usuariosTotal = usuariosIds.Append(usuarioId).Distinct();
        var existentes = await _userManager.Users.Where(x=> usuariosTotal.Contains(x.Id)).Select(x=>x.Id).ToListAsync();
        return existentes.Count == usuariosTotal.Count();
    }

}