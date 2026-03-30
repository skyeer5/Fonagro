using MediatR;
using Web.Application.Interfaces;
using WebApp.Application.Authentication.Command.Login;
using WebApp.Application.Core;
using static WebApp.Application.Authentication.Command.ChangePassword.ChangePasswordCommand;

namespace Web.Application.Authentication.Command.ChangePassword;

public class ChangePasswordCommandHandler : IRequestHandler<ChangePasswordCommandRequest, Result<bool>>
{
    private readonly IAuthService _authService;
    public ChangePasswordCommandHandler(IAuthService authService)
    {
        _authService = authService;
    }
    public async Task<Result<bool>> Handle(ChangePasswordCommandRequest request, CancellationToken cancellationToken)
    {
        return await _authService.ChangePasswordAsync(request.changePasswordRequest.AnteriorPassword, request.changePasswordRequest.Password);
    }
}