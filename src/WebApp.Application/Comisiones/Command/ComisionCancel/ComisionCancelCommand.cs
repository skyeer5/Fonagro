using MediatR;
using WebApp.Application.Core;

namespace WebApp.Application.Comisiones.Command.ComisionCancel;

public class ComisionCancelCommand
{
    public record ComisionCancelCommandRequest(ComisionCancelRequest ComisionCancelRequest) : IRequest<Result<int>>;
    
}