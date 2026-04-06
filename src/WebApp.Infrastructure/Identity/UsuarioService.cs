using System.Linq.Expressions;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Azure;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WebApp.Application.Core;
using WebApp.Application.Interfaces;
using WebApp.Application.Usuarios.Commands.UsuarioCreate;
using WebApp.Application.Usuarios.Queries.GetUsuariosActivos;
using WebApp.Application.Usuarios.Queries.GetUsuariosActivosDetalle;
using WebApp.Application.Usuarios.Queries.GetUsuariosSinComision;
using WebApp.Domain;
using WebApp.Persistence;
using WebApp.Persistence.Models;
using static WebApp.Application.Usuarios.Queries.GetUsuariosActivosDetalle.GetUsuariosActivosDetalleQuery;

namespace WebApp.Infrastructure.Identity;

public class UsuarioService : IUsuarioService
{
    private readonly WebAppDbContext _context;
    private readonly UserManager<AppUser> _userManager;
    private readonly IMapper _mapper;

    public UsuarioService(UserManager<AppUser> userManager, WebAppDbContext context, IMapper mapper)
    {
        _userManager = userManager;
        _context = context;
        _mapper = mapper;
    }

    public async Task<string?> GetNombreUsuarioAsync(int usuarioId)
    {
        return await _userManager.Users.Where(u => u.Id == usuarioId)
            .Select(u => u.Nombre_Completo)
            .FirstOrDefaultAsync();
    }

    public async Task<List<GetUsuariosActivosResponse>> GetUsuariosActivosAsync()
    {
        return await _userManager.Users
            .Where(u => u.Estado == UsuarioEstados.Activo)
            .Select(u => new GetUsuariosActivosResponse
            {
                Id = u.Id,
                Nombre_Completo = u.Nombre_Completo
            })
            .ToListAsync();
    }

    public async Task<Result<PagedList<GetUsuariosActivosDetalleResponse>>> GetUsuariosActivosDetalleAsync(GetUsuariosActivosDetalleQueryRequest request)
    {
        IQueryable<AppUser> queryable = _userManager.Users;

        var predicate = ExpressionBuilder.New<AppUser>();

            predicate = predicate
                        .And(x=>x.Estado == UsuarioEstados.Activo);
        if(!string.IsNullOrEmpty(request.request.Nombre))
        {
            predicate = predicate
                        .And(x=>x.Nombre_Completo!
                        .Contains(request.request.Nombre)
                        );
        }
        if(!string.IsNullOrEmpty(request.request.OrderBy))
        {
            Expression<Func<AppUser, object>> orderBySelector = 
                        request.request.OrderBy.ToLower() switch
                        {
                            "nombre" => user => user.Nombre_Completo!,
                            "puesto" => user => user.Puesto!,
                            "unidad" => user => user.Unidad!,
                            _ => user => user.Nombre_Completo!
                        };
            bool orderBy = request.request.OrderAsc.HasValue 
                            ? request.request.OrderAsc.Value :
                            true;
            queryable = orderBy ? queryable.OrderBy(orderBySelector) : queryable.OrderByDescending(orderBySelector);
        }
        queryable = queryable.Where(predicate);

        var usersQuery = queryable.ProjectTo<GetUsuariosActivosDetalleResponse>(_mapper.ConfigurationProvider).AsQueryable();
        var pagination = await PagedList<GetUsuariosActivosDetalleResponse>.CreateAsync(
                                        usersQuery,
                                        request.request.PageNumber,
                                        request.request.PageSize
            
        );
        return Result<PagedList<GetUsuariosActivosDetalleResponse>>.Success(pagination);
    }

    public async Task<List<GetUsuariosSinComisionResponse>> GetUsuariosSinComisionAsync()
    { 
        return await _userManager.Users
            .Where(u => u.Estado == UsuarioEstados.Activo && !u.ComisionUsuarios!.Any(cu => cu.Comision!.Estado != EstadosTipos.Finalizado && cu.Comision.Estado != EstadosTipos.Cancelada))
            .OrderBy(x=> x.Nombre_Completo)
            .Select(u => new GetUsuariosSinComisionResponse
            {
                Id = u.Id,
                Nombre_Completo = u.Nombre_Completo
            })
            .ToListAsync();
    }

    public async Task<bool> UsuariosExistsAsync(int usuarioId)
    {
        return await _userManager.FindByIdAsync(usuarioId.ToString()) != null;
    }

    public async Task<bool> UsuariosExistsAsync(int usuarioId, List<int> usuariosIds)
    {
        var usuariosTotal = usuariosIds.Append(usuarioId).Distinct();
        var existentes = await _userManager.Users.Where(x=> usuariosTotal.Contains(x.Id)).Select(x=>x.Id).ToListAsync();
        return existentes.Count == usuariosTotal.Count();
    }

}