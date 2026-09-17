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
        
        RuleFor(x => x.Hora_Salida)
            .NotEmpty().WithMessage("La hora de salida es requerida.");

        RuleFor(x => x.Hora_Regreso)
            .NotEmpty().WithMessage("La hora de regreso es requerida.");   
    }
}