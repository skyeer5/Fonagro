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
        RuleFor(x => x.Fecha_Regreso)
            .NotEmpty().WithMessage("La fecha de regreso no debe estar vacía.");

        RuleFor(x => x.Fecha_Salida)
            .GreaterThan(DateTime.Now).WithMessage("La fecha de salida debe ser posterior a la fecha actual.");
        RuleFor(x => x.Fecha_Salida)
            .NotEmpty().WithMessage("La fecha de salida no debe estar vacía.");

        RuleFor(x => x.VehiculoId)
            .NotEmpty().WithMessage("Se debe seleccionar un vehículo para la comisión.");
        
        RuleFor(x => x.UsuariosNom)
            .NotEmpty().WithMessage("La lista de usuarios no debe estar vacía.");
        RuleFor(x=>x.UsuariosNom)
            .Must(x => x.Count(u => u.Es_Piloto == true) == 1)
            .WithMessage("Debe haber exactamente un piloto en la lista de usuarios nombrados.");
        RuleFor(x => x.UsuariosNom)
            .Must(x=>x.Count<=5 && x.Count>0).WithMessage("La lista de usuarios debe contener entre 1 y 5.");
        RuleFor(x => x.UsuariosNom)
            .Must(x => x.Select(u => u.UsuariosId).Distinct().Count() == x.Count)
            .WithMessage("No puede haber usuarios duplicados en la lista de usuarios nombrados.");

        RuleFor(x => x.Departamento)
            .NotEmpty().WithMessage("El departamento no debe estar vacío.");
        
    }
}