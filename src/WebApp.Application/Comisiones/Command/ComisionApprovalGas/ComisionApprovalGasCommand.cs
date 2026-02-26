using MediatR;
using WebApp.Application.Core;

namespace WebApp.Application.Comisiones.Command.ComisionApprovalGas;

public class ComisionApprovalGasCommand
{
    public record ComisionApprovalGasCommandRequest(ComisionApprovalGasRequest ComisionApprovalGasRequest) : IRequest<Result<int>>;
    
}