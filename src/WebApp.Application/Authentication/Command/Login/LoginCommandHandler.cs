using MediatR;
using Web.Application.Interfaces;
using WebApp.Application.Authentication.Command.Login;
using WebApp.Application.Core;

namespace Web.Application.Authentication.Command.Login;

public class LoginCommandHandler : IRequestHandler<LoginCommand.LoginCommandRequest, Result<bool>>
{
    private readonly IAuthService _authService;
    public LoginCommandHandler(IAuthService authService)
    {
        _authService = authService;
    }
    public async Task<Result<bool>> Handle(LoginCommand.LoginCommandRequest request, CancellationToken cancellationToken)
    {
        var resultado = await _authService.LoginAsync(request.loginRequest.NIT, request.loginRequest.Password);
        if(!resultado.IsSuccess)
        {
            return Result<bool>.Failure(resultado.Error!);
        }

        return Result<bool>.Success(resultado.Value!);
    }
}