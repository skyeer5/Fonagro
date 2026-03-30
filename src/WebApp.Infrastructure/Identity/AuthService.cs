using Microsoft.AspNetCore.Identity;
using Web.Application.Authentication.Command.Login;
using Web.Application.Interfaces;
using WebApp.Application.Authentication.Command.Login;
using WebApp.Application.Core;
using WebApp.Application.Interfaces;
using WebApp.Domain;
using WebApp.Persistence.Models;

namespace WebApp.Infrastructure.Identity;

public class AuthService : IAuthService
{
    private readonly UserManager<AppUser> _userManager;
    private readonly SignInManager<AppUser> _signInManager;
    private readonly ICurrentUser _currentUser;
    public AuthService(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, ICurrentUser currentUser)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _currentUser = currentUser;
    }

    public async Task<Result<bool>> ChangePasswordAsync(string? AnteriorPassword, string? Password)
    {
        var userId = _currentUser.userId;

        var user = await _userManager.FindByIdAsync(userId.ToString());
        if (user is null)
            return Result<bool>.Failure("Usuario no encontrado");

        var result = await _userManager.ChangePasswordAsync(user, AnteriorPassword!,  Password!);
        if (!result.Succeeded)
        {
            var stringErrors = "Errores al cambiar la contraseña: " + string.Join(", ", result.Errors.Select(e => e.Description));
            return Result<bool>.Failure(stringErrors);
        }

        user.Estado = UsuarioEstados.Activo;
        await _userManager.UpdateAsync(user);

        await _signInManager.RefreshSignInAsync(user);

        return Result<bool>.Success(true);

    }

    public async Task<Result<LoginResponse>> LoginAsync(string? nit, string? password)
    {
        var user = await _userManager.FindByNameAsync(nit!);

        if (user is null)
            return Result<LoginResponse>.Failure("Usuario no encontrado");

        var result = await _signInManager.PasswordSignInAsync(
            user,
            password!,
            isPersistent: true,
            lockoutOnFailure: true
        );
        if (!result.Succeeded)
            return Result<LoginResponse>.Failure("Credenciales incorrectas");
            
        return Result<LoginResponse>.Success(new LoginResponse
        {
            PideCambioContrasena = user.Estado == UsuarioEstados.PendientePrimerAcceso
        });
    }
}