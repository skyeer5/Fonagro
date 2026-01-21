using FluentValidation;
using MediatR;
using WebApp.Application.Core;
using static WebApp.Application.Vehiculos.Commands.VehiculoCreate.VehiculoCreateCommand;

namespace WebApp.Application.Vehiculos.Commands.VehiculoCreate;

public class VehiculoCreateCommand
{    public record VehiculoCreateCommandRequest(VehiculoCreateRequest VehiculoCreateRequest) : IRequest<Result<int>>;
    
}

public class VehiculoCreateCommandRequestValidator : AbstractValidator<VehiculoCreateCommandRequest>
    {
        public VehiculoCreateCommandRequestValidator()
        {
            RuleFor(x => x.VehiculoCreateRequest).SetValidator(new VehiculoCreateValidator());
        }
    }