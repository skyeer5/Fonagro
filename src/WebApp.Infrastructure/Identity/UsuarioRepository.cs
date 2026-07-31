using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WebApp.Application.Core;
using WebApp.Application.Interfaces;
using WebApp.Application.Usuarios.Commands.UsuarioCreate;
using WebApp.Domain;
using WebApp.Domain.Usuarios;
using WebApp.Persistence.Models;

namespace WebApp.Infrastructure.Identity;

public class UsuarioRepository : IUsuarioRepository
{
    private readonly UserManager<AppUser> _userManager;
    private readonly RoleManager<IdentityRole<int>> _roleManager;

    public UsuarioRepository(UserManager<AppUser> userManager, RoleManager<IdentityRole<int>> roleManager)
    {
        _userManager = userManager;
        _roleManager = roleManager;
    }

    public async Task<Result<int>> CreateUsuarioAsync(UsuarioCreateRequest request, CancellationToken cancellationToken)
    {
        var usuario = AppUser.Crear(
            request.Nombres!,
            request.Apellidos!,
            request.NIT!,
            request.Tipo_Servicios,
            request.Numero_Contrato!,
            request.Email!,
            request.Puesto
            );
        var result_create = await _userManager.CreateAsync(usuario, request.Password!);
        if (!result_create.Succeeded)
        {
            var stringErrors = "Errores al crear el usuario: " + string.Join(", ", result_create.Errors.Select(e => e.Description));
            return Result<int>.Failure(stringErrors);
        }

        return Result<int>.Success(usuario.Id);
    }
}