using FluentValidation;

namespace WebApp.Application.Nombramientos.Command.NombramientoCreate;

public class NombramientoCreateValidator : AbstractValidator<NombramientoCreateRequest>
{
    public NombramientoCreateValidator()
    {
        RuleFor(x => x.Proposito)
            .NotEmpty().WithMessage("El propósito es requerido.");

        RuleFor(x => x.Fecha_Salida)
            .NotEmpty().WithMessage("La fecha de salida es requerida.");

        RuleFor(x => x.Fecha_Regreso)
            .NotEmpty().WithMessage("La fecha de regreso es requerida.")
            .GreaterThanOrEqualTo(x => x.Fecha_Salida).WithMessage("La fecha de regreso debe ser mayor o igual a la fecha de salida.");

        RuleFor(x => x.UsuarioId)
            .NotEmpty().WithMessage("El usuario es requerido.");

        RuleFor(x => x.Municipios)
            .NotEmpty().WithMessage("Se requiere al menos un municipio.");
    }
}