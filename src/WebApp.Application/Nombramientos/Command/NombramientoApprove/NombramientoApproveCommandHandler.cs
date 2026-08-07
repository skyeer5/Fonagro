using MediatR;
using WebApp.Application.Core;
using WebApp.Application.Interfaces;
using static WebApp.Application.Nombramientos.Command.NombramientoApprove.NombramientoApproveCommand;

namespace WebApp.Application.Nombramientos.Command.NombramientoApprove;

public class NombramientoApproveCommandHandler : IRequestHandler<NombramientoApproveCommandRequest, Result<int>>
{
    private readonly INombramientoRepository _nombramientoRepository;

    public NombramientoApproveCommandHandler(INombramientoRepository nombramientoRepository)
    {
        _nombramientoRepository = nombramientoRepository;
    }

    public async Task<Result<int>> Handle(NombramientoApproveCommandRequest request, CancellationToken cancellationToken)
    {
        var result = await _nombramientoRepository.AprobarNombramientoAsync(request.request.NombramientoId, cancellationToken);
        if(result == 0)
        {
            return Result<int>.Failure("Ocurrio un error al intentar aprobar esta comisión");
        }
        return Result<int>.Success(result);
    }
}