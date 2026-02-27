using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WebApp.Application.Core;
using WebApp.Application.Interfaces;
using WebApp.Application.Usuarios.Commands.UsuarioCreate;
using WebApp.Application.Usuarios.Queries.GetUsuariosActivos;
using WebApp.Application.Usuarios.Queries.GetUsuariosSinComision;
using WebApp.Domain;
using WebApp.Persistence;
using WebApp.Persistence.Models;

namespace WebApp.Infrastructure.Identity;

public class UsuarioService : IUsuarioService
{
    private readonly WebAppDbContext _context;
    private readonly UserManager<AppUser> _userManager;

    public UsuarioService(UserManager<AppUser> userManager, WebAppDbContext context)
    {
        _userManager = userManager;
        _context = context;
    }

    public async Task<string?> GetNombreUsuarioAsync(int usuarioId)
    {
        return await _userManager.Users.Where(u => u.Id == usuarioId)
            .Select(u => u.Nombre_Completo)
            .FirstOrDefaultAsync();
    }

    public async Task<List<GetUsuariosActivosResponse>> getUsuariosActivosAsync()
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

    public async Task<List<GetUsuariosSinComisionResponse>> getUsuariosSinComisionAsync()
    { 
        return await _userManager.Users
            .Where(u => u.Estado == UsuarioEstados.Activo && !u.ComisionUsuarios!.Any(cu => cu.Comision!.Estado != EstadosTipos.Finalizado))
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