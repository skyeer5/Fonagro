using System.Data;
using FluentValidation;
using WebApp.Application.Comisiones.ComisionCreate;

namespace WebApp.Application.Comision.ComisionCreate;

public class ComisionCreateValidator : AbstractValidator<ComisionCreateRequest>
{
    public ComisionCreateValidator()
    {

        RuleFor(x => x.Fecha_Regreso)
            .GreaterThan(x => x.Fecha_Salida).WithMessage("La fecha de regreso debe ser posterior a la fecha de salida.");

        RuleFor(x => x.Fecha_Salida)
            .GreaterThan(DateTime.Now).WithMessage("La fecha de salida debe ser posterior a la fecha actual.");

        RuleFor(x => x.UsuarioId)
            .GreaterThan(0).WithMessage("El ID del usuario debe ser un número positivo.");

        RuleFor(x => x.VehiculoId)
            .GreaterThan(0).WithMessage("El ID del vehículo debe ser un número positivo.");
        
        RuleFor(x => x.UsuariosNombrados)
            .NotEmpty().WithMessage("La lista de IDs de usuarios no debe estar vacía.");
            
        RuleFor(x => x.Departamento)
            .NotEmpty().WithMessage("El departamento no debe estar vacío.");
        
        RuleFor(x => x.UsuariosNombrados)
            .Must(x=>x.Count<=5 && x.Count>0).WithMessage("La lista de IDs de usuarios debe contener entre 1 y 5.");
        
        

    }
}