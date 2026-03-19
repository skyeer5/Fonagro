using Microsoft.AspNetCore.Identity;
using Web.Application.Authentication.Command.Login;
using Web.Application.Interfaces;
using WebApp.Application.Core;
using WebApp.Persistence.Models;

namespace WebApp.Infrastructure.Identity;

public class AuthService : IAuthService
{
    private readonly UserManager<AppUser> _userManager;
    private readonly SignInManager<AppUser> _signInManager;
    public AuthService(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager)
    {
        _userManager = userManager;
        _signInManager = signInManager;
    }
    public async Task<Result<bool>> LoginAsync(string? nit, string? password)
    {
        var user = await _userManager.FindByNameAsync(nit!);

        if (user is null)
            return Result<bool>.Failure("Usuario no encontrado");

        var result = await _signInManager.PasswordSignInAsync(
            user,
            password!,
            isPersistent: true,
            lockoutOnFailure: true
        );
        if (!result.Succeeded)
            return Result<bool>.Failure("Credenciales incorrectas");
            
        return Result<bool>.Success(result.Succeeded);
    }
}