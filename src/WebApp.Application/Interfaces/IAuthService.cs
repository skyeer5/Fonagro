using Web.Application.Authentication.Command.Login;
using WebApp.Application.Authentication.Command.Login;
using WebApp.Application.Core;

namespace Web.Application.Interfaces;

public interface IAuthService
{
    Task<Result<LoginResponse>> LoginAsync(string? NIT, string? Password);
    Task<Result<bool>> ChangePasswordAsync(string? AnteriorPassword, string? Password);
}