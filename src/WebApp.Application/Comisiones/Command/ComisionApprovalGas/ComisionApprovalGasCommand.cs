using FluentValidation;
using MediatR;
using WebApp.Application.Core;
using static WebApp.Application.Comisiones.Command.ComisionApprovalGas.ComisionApprovalGasCommand;

namespace WebApp.Application.Comisiones.Command.ComisionApprovalGas;

public class ComisionApprovalGasCommand
{
    public record ComisionApprovalGasCommandRequest(ComisionApprovalGasRequest ComisionApprovalGasRequest) : IRequest<Result<int>>;
    
}
public class ComisionApprovalGasCommandRequestValidator : AbstractValidator<ComisionApprovalGasCommandRequest>
{
    public ComisionApprovalGasCommandRequestValidator()
    {
        RuleFor(x=>x.ComisionApprovalGasRequest).SetValidator(new ComisionApprovalGasValidator());
    }
}