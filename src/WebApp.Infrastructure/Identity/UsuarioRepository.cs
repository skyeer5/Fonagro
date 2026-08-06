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
    private readonly ICurrentUser _currentUser;

    public UsuarioRepository(UserManager<AppUser> userManager, RoleManager<IdentityRole<int>> roleManager, ICurrentUser currentUser)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _currentUser = currentUser;
    }

    public async Task<Result<int>> CreateUsuarioAsync(UsuarioCreateRequest request, CancellationToken cancellationToken)
    {
        var user = _currentUser.userId;
        var usuario = AppUser.Crear(
            request.Nombres!,
            request.Apellidos!,
            request.NIT!,
            request.Tipo_Servicios,
            request.Numero_Contrato!,
            request.Email!,
            request.Puesto,
            user
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