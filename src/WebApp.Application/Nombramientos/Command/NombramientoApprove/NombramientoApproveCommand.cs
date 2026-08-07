using MediatR;
using WebApp.Application.Core;

namespace WebApp.Application.Nombramientos.Command.NombramientoApprove;

public class NombramientoApproveCommand
{
    public record NombramientoApproveCommandRequest(NombramientoApproveRequest request) : IRequest<Result<int>>;
}