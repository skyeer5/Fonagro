using FluentValidation;
using MediatR;
using WebApp.Application.Core;
using static WebApp.Application.Authentication.Command.ChangePassword.ChangePasswordCommand;

namespace WebApp.Application.Authentication.Command.ChangePassword;

public class ChangePasswordCommand
{
    public record ChangePasswordCommandRequest(ChangePasswordRequest changePasswordRequest) : IRequest<Result<bool>>;
}

public class ChangePasswordCommandRequestValidator : AbstractValidator<ChangePasswordCommandRequest>
{
    public ChangePasswordCommandRequestValidator()
    {
        RuleFor(x => x.changePasswordRequest).SetValidator(new ChangePasswordValidator());
    }
}