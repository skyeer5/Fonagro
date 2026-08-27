using FluentValidation;

namespace WebApp.Application.Comisiones.Command.ComisionApprovalGas;

public class ComisionApprovalGasValidator : AbstractValidator<ComisionApprovalGasRequest>
{
    public ComisionApprovalGasValidator()
    {
        RuleFor(x=>x.PrespuestoAprobado)
            .Must(p=> p % 50 == 0).WithMessage("El prespupuesto debe ser un valor divisible en 50.");
    }
}