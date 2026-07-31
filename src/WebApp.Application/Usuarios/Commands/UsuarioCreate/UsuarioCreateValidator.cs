using FluentValidation;

namespace WebApp.Application.Usuarios.Commands.UsuarioCreate;

public class UsuarioCreateValidator : AbstractValidator<UsuarioCreateRequest>
{
    public UsuarioCreateValidator()
    {
        RuleFor(x => x.Nombres)
            .NotEmpty().WithMessage("El nombre es requerido.")
            .MaximumLength(100).WithMessage("El nombre no puede exceder los 100 caracteres.");

        RuleFor(x => x.Apellidos)
            .NotEmpty().WithMessage("El apellido es requerido.")
            .MaximumLength(100).WithMessage("El apellido no puede exceder los 100 caracteres.");

        RuleFor(x => x.NIT)
            .NotEmpty().WithMessage("El NIT es requerido.")
            .MaximumLength(20).WithMessage("El NIT no puede exceder los 20 caracteres.");

        RuleFor(x => x.Puesto)
            .NotEmpty().WithMessage("El puesto es requerido.");

        RuleFor(x => x.Tipo_Servicios)
            .NotEmpty().WithMessage("El tipo de servicios es requerido.");

        RuleFor(x => x.Numero_Contrato)
            .NotEmpty().WithMessage("El número de contrato es requerido.")
            .MaximumLength(20).WithMessage("El número de contrato no puede exceder los 20 caracteres.");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("El email es requerido.")
            .EmailAddress().WithMessage("El email no es válido.")
            .MaximumLength(100).WithMessage("El email no puede exceder los 100 caracteres.");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("La contraseña es requerida.");
    }
}