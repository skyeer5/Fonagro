using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WebApp.Application.Core;
using WebApp.Application.Interfaces;
using WebApp.Application.Usuarios.Commands.UsuarioCreate;
using WebApp.Domain;
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
            nombreCompleto: request.Nombre_Completo!,
            nit: request.NIT!,
            puesto: request.Puesto!,
            unidad: request.Unidad!,
            tipoServicios: request.Tipo_Servicios!,
            numeroContrato: request.Numero_Contrato!,
            email: request.Email!
            );
        var roles = await _roleManager.Roles
                            .Select(x=>x.Name)
                            .ToListAsync(cancellationToken);

        var rol = roles.FirstOrDefault(r => r == RolesTipos.Usuario);  

        if(request.Puesto!.Contains(UsuariosTipos.Encargado_Administracion))
        {
            rol = roles.FirstOrDefault(r => r == RolesTipos.Aprobador_Gasolina);
        }
        else if(request.Puesto!.Contains(UsuariosTipos.Encargado_Servicios) || request.Puesto!.Contains(UsuariosTipos.Auxiliar_Servicios))
        {
            rol = roles.FirstOrDefault(r => r == RolesTipos.Comisionista);
        }
        var result_create = await _userManager.CreateAsync(usuario, request.Password!);
        var resultat_add_role = await _userManager.AddToRoleAsync(usuario, rol!);
        if (!result_create.Succeeded)
        {
            var stringErrors = "Errores al crear el usuario: " + string.Join(", ", result_create.Errors.Select(e => e.Description));
            return Result<int>.Failure(stringErrors);
        }

        if (!resultat_add_role.Succeeded)
        {
            var stringErrors = "Errores al asignar el rol al usuario: " + string.Join(", ", resultat_add_role.Errors.Select(e => e.Description));
            return Result<int>.Failure(stringErrors);
        }

        return Result<int>.Success(usuario.Id);
    }
}