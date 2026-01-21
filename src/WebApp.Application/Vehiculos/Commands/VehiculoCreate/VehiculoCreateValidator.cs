using FluentValidation;

namespace WebApp.Application.Vehiculos.Commands.VehiculoCreate;

public class VehiculoCreateValidator : AbstractValidator<VehiculoCreateRequest>
{
    public VehiculoCreateValidator()
    {
        RuleFor(x => x.Placa)
            .NotEmpty().WithMessage("La placa es obligatoria.")
            .MaximumLength(50).WithMessage("La placa no puede exceder los 50 caracteres.");
        RuleFor(x => x.Modelo)
            .NotEmpty().WithMessage("El modelo es obligatorio.")
            .MaximumLength(50).WithMessage("El modelo no puede exceder los 50 caracteres.");
    }
}