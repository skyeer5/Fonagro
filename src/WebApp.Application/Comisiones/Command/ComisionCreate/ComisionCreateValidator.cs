using System.Data;
using FluentValidation;
using WebApp.Application.Comisiones.ComisionCreate;

namespace WebApp.Application.Comision.ComisionCreate;

public class ComisionCreateValidator : AbstractValidator<ComisionCreateRequest>
{
    public ComisionCreateValidator()
    {        
        RuleFor(x => x.Nombramientos)
            .NotEmpty().WithMessage("La lista de usuarios no debe estar vacía.")
            .Must(x=>x.Count<=5 && x.Count>0).WithMessage("La lista de usuarios debe contener entre 1 y 5.");        
    }
}