using MediatR;
using Web.Application.Interfaces;
using WebApp.Application.Authentication.Command.Login;
using WebApp.Application.Core;

namespace Web.Application.Authentication.Command.Login;

public class LoginCommandHandler : IRequestHandler<LoginCommand.LoginCommandRequest, Result<LoginResponse>>
{
    private readonly IAuthService _authService;
    public LoginCommandHandler(IAuthService authService)
    {
        _authService = authService;
    }
    public async Task<Result<LoginResponse>> Handle(LoginCommand.LoginCommandRequest request, CancellationToken cancellationToken)
    {
        return await _authService.LoginAsync(request.loginRequest.NIT, request.loginRequest.Password);

    }
}