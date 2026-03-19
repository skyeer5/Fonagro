using Web.Application.Authentication.Command.Login;
using WebApp.Application.Core;

namespace Web.Application.Interfaces;

public interface IAuthService
{
    Task<Result<bool>> LoginAsync(string? NIT, string? Password);
}