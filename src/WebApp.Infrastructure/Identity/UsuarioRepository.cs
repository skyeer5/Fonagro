using Microsoft.AspNetCore.Identity;
using WebApp.Application.Core;
using WebApp.Application.Interfaces;
using WebApp.Application.Usuarios.Commands.UsuarioCreate;
using WebApp.Domain;
using WebApp.Persistence.Models;

namespace WebApp.Infrastructure.Identity;

public class UsuarioRepository : IUsuarioRepository
{
    private readonly UserManager<AppUser> _userManager;

    public UsuarioRepository(UserManager<AppUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task<Result<int>> CreateUsuarioAsync(UsuarioCreateRequest request, CancellationToken cancellationToken)
    {
        var usuario = new AppUser
        {
            Nombre_Completo = request.Nombre_Completo,
            NIT = request.NIT,
            Puesto = request.Puesto,
            Unidad = request.Unidad,
            Tipo_Servicios = request.Tipo_Servicios,
            Numero_Contrato = request.Numero_Contrato,
            Estado = UsuarioEstados.Activo,
            Email = request.Email,
            UserName = request.Email
        };
        var result = await _userManager.CreateAsync(usuario, request.Password!);
        if (!result.Succeeded)
        {
            var stringErrors = "Errores al crear el usuario: " + string.Join(", ", result.Errors.Select(e => e.Description));
            return Result<int>.Failure(stringErrors);
        }

        return Result<int>.Success(usuario.Id);
    }
}