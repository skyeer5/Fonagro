using MediatR;
using Web.Application.Authentication.Command.Login;
using WebApp.Application.Core;

namespace WebApp.Application.Authentication.Command.Login;

public class LoginCommand
{
    public record LoginCommandRequest(LoginRequest loginRequest) : IRequest<Result<bool>>;
}