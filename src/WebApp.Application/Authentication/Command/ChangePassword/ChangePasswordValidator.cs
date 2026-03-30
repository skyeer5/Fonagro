using FluentValidation;

namespace WebApp.Application.Authentication.Command.ChangePassword;

public class ChangePasswordValidator : AbstractValidator<ChangePasswordRequest>
{
    public ChangePasswordValidator()
    {
        RuleFor(x => x.AnteriorPassword)
            .NotEmpty().WithMessage("La contraseña actual es requerida.");

        RuleFor(x => x.ConfirmPassword)
            .NotEmpty().WithMessage("La confirmación de la nueva contraseña es requerida.")
            .Equal(x => x.Password).WithMessage("La confirmación de la nueva contraseña no coincide con la nueva contraseña.");
    }
}